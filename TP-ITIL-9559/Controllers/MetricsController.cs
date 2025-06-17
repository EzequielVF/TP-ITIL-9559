using TP_ITIL_9559.Data;
using TP_ITIL_9559.Model;
using Microsoft.AspNetCore.Mvc;

namespace TP_ITIL_9559.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : Controller
    {
        public ITILDbContext DbContext { get; set; }
        public MetricsController(ITILDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet("incidents/{days?}")]
        public IActionResult IncidentMetrics(int days = 30)
        {
            var incidents = DbContext.Incidents;
            if (incidents == null) { return NotFound(); }
            var incidentMetrics = GetIncidentMetrics(days);
            return Ok(incidentMetrics);
        }

        private IncidentMetricsDto GetIncidentMetrics(int days)
        {
            DateTime startDate = DateTime.UtcNow.AddDays(-days);
            DateTime endDate = DateTime.UtcNow;

            DateTime startDateForHours = DateTime.UtcNow.AddDays(-9);
            int totalHours = (int)(endDate - startDateForHours).TotalHours;

            float[] incidentsPerDay = new float[7];
            float[] incidentsPerHour = new float[24];

            var mostAfectedItemKey = DbContext.Incidents
                .Where(i => i.CreatedDate >= startDate && i.CreatedDate <= endDate)
                .GroupBy(i => i.ConfigurationItemId)
                .Select(g => new { Item = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefault();

            var mostAfectedItemName = DbContext.Configuration
                .Where(i => i.Id == mostAfectedItemKey.Item).FirstOrDefault();

            //se agrupa por cada dia de la semana y se suma cuantos incidentes hay cada uno de esos dias.
            var groupByDayOfWeek = DbContext.Incidents
                .Where(i => i.CreatedDate >= startDate && i.CreatedDate <= endDate)
                .GroupBy(i => i.CreatedDate.DayOfWeek)
                .Select(g => new { DayOfWeek = g.Key, Count = g.Count() });

            int[] incidentsCountPerDayOfWeek = new int[7]; // Array to store the result

            //inicializamos cada dia de la semana en 0.
            for (int i = 0; i < 7; i++)
            {
                incidentsCountPerDayOfWeek[i] = 0;
            }

            //completamos con la info de la query almacenada en groupByDayOfWeek
            foreach (var item in groupByDayOfWeek)
            {
                int dayOfWeek = (int)item.DayOfWeek;
                incidentsCountPerDayOfWeek[dayOfWeek] = item.Count;
            }

            //acumulo por la cantidad total de cada dia.
            for (int i = 0; i < incidentsCountPerDayOfWeek.Length; i++)
            {
                incidentsPerDay[i] = (float)incidentsCountPerDayOfWeek[i];
            }

            var groupByHourOfDay = DbContext.Incidents
                .Where(i => i.CreatedDate >= startDate && i.CreatedDate <= endDate)
                .GroupBy(i => (i.CreatedDate.Hour))
                .Select(g => new { Hour = g.Key, Count = g.Count() });

            float[] incidentsCountPerHourOfDay = new float[24]; // Array to store the result

            //inicializamos cada hora del dia en 0.
            for (int i = 0; i < 24; i++)
            {
                incidentsCountPerHourOfDay[i] = 0;
            }

            //completamos con la info de la query almacenada en groupByHourOfDay
            foreach (var item in groupByHourOfDay)
            {
                int hour = RestarHoraConRollover(item.Hour, 3);
                incidentsCountPerHourOfDay[hour] = (float)item.Count;
            }

            //esta division no la hacemos porque quedaria un numero muy pequeño.
            for (int i = 0; i < incidentsCountPerHourOfDay.Length; i++)
            {
                incidentsPerHour[i] = (float)incidentsCountPerHourOfDay[i] / totalHours;
            }

            int maxDay = incidentsCountPerDayOfWeek.Max();
            int maxIndex = incidentsCountPerDayOfWeek.ToList().IndexOf(maxDay);
            string dayWithMostIncidents = new DateTime(2024, 6, 2 + maxIndex, 0, 0, 0)
    .ToString("dddd", new System.Globalization.CultureInfo("es-ES"));

            int maxHour = (int)incidentsCountPerHourOfDay.Max();
            int hourWithMostIncidents = incidentsCountPerHourOfDay.ToList().IndexOf(maxHour);

            TimeSpan totalResolutionTime = TimeSpan.Zero;
            int closedIncidentsCount = DbContext.Incidents
                .Count(i => i.State == "IMPLEMENTADO" && i.ClosedDate >= startDate && i.ClosedDate <= endDate);

            if (closedIncidentsCount > 0)
            {
                totalResolutionTime = TimeSpan.FromMilliseconds(DbContext.Incidents
                    .Where(i => i.State == "IMPLEMENTADO" && i.ClosedDate >= startDate && i.ClosedDate <= endDate)
                    .Sum(i => (i.ClosedDate - i.CreatedDate).TotalMilliseconds));
            }

            double avgResolutionTime = 0.0;
            if (closedIncidentsCount > 0)
            {
                avgResolutionTime = totalResolutionTime.TotalHours / closedIncidentsCount;
            }

            var incidentMetrics = new IncidentMetricsDto()
            {
                IncidentsPerDay = incidentsPerDay,
                IncidentsPerHour = incidentsCountPerHourOfDay,
                DayWithMostIncidents = dayWithMostIncidents,
                HourWithMostIncidents = hourWithMostIncidents,
                AvgResolutionTime = avgResolutionTime,
                mostAfectedItemName = mostAfectedItemName?.Title ?? "Unknown"
            };

            return incidentMetrics;
        }
        private int RestarHoraConRollover(int hora, int resta)
        {
            int resultado = (hora - resta) % 24;
            if (resultado < 0)
                resultado += 24;
            return resultado;
        }
    }
}