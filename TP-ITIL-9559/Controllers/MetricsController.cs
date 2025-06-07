using TP_ITIL_9559.Data;
using TP_ITIL_9559.Model;
using Microsoft.AspNetCore.Mvc;

namespace TP_ITIL_9559.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController: Controller
    {
        public ITILDbContext DbContext {get;set;}
        public MetricsController(ITILDbContext dbContext){
            DbContext = dbContext;
        }

        [HttpGet("incidents/{days}")]
        public IActionResult IncidentMetrics(int days)
        {
            var incidents = DbContext.Incidents;
            if (incidents == null) {return NotFound();}
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

            //se agrupa por cada dia de la semana y se suma cuantos incidentes hay cada uno de esos dias.
            var groupByDayOfWeek = DbContext.Incidents
                .Where(i => i.CreatedDate >= startDate && i.CreatedDate <= endDate)
                .GroupBy(i => i.CreatedDate.DayOfWeek)
                .Select(g => new { DayOfWeek = g.Key, Count = g.Count() });

            int[] incidentsCountPerDayOfWeek = new int[7]; // Array to store the result

            //inicializamos cada dia de la semana en 0.
            for(int i = 0; i < 7; i++)
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

            //se agrupa por cada hora del dia y se suma cuantos incidentes hay cada uno de esas horas.
            //se resta 3 para que sea hora local.
            var groupByHourOfDay= DbContext.Incidents
                .Where(i => i.CreatedDate >= startDate && i.CreatedDate <= endDate)
                .GroupBy(i => (i.CreatedDate.Hour - 3))
                .Select(g => new { Hour = g.Key, Count = g.Count() });

            float[] incidentsCountPerHourOfDay = new float[24]; // Array to store the result

            //inicializamos cada hora del dia en 0.
            for(int i = 0; i < 24; i++)
            {
                incidentsCountPerHourOfDay[i] = 0;
            }

            //completamos con la info de la query almacenada en groupByHourOfDay
            foreach (var item in groupByHourOfDay)
            {
                int hour = item.Hour;
                incidentsCountPerHourOfDay[hour] = (float)item.Count;
            }

            //esta division no la hacemos porque quedaria un numero muy pequeño.
            for (int i = 0; i < incidentsCountPerHourOfDay.Length; i++)
            {
                incidentsPerHour[i] = (float)incidentsCountPerHourOfDay[i] / totalHours;
            }

            int maxDay = incidentsCountPerDayOfWeek.Max();
            int maxIndex = incidentsCountPerDayOfWeek.ToList().IndexOf(maxDay);
            string dayWithMostIncidents = Enum.GetName(typeof(DayOfWeek), maxIndex) ?? "Unknown";

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

            TimeSpan avgResolutionTime = TimeSpan.Zero;
            if (closedIncidentsCount > 0)
            {
                avgResolutionTime = TimeSpan.FromTicks(totalResolutionTime.Ticks / closedIncidentsCount);
            }

            var incidentMetrics = new IncidentMetricsDto()
            {
                //se obtiene haciendo la suma total de incidentes para cada dia
                //tomando los ultimos 30 dias, y dividiendo esa suma por 30 que es
                //la cantidad total de dias.
                IncidentsPerDay = incidentsPerDay,
                //se obtiene haciendo la suma total de incidentes para cada hora
                //tomando los ultimos 7 dias.
                IncidentsPerHour = incidentsCountPerHourOfDay,
                DayWithMostIncidents = dayWithMostIncidents,
                HourWithMostIncidents = hourWithMostIncidents,
                AvgResolutionTime = avgResolutionTime.ToString()
            };

            return incidentMetrics;
        }
    }
}