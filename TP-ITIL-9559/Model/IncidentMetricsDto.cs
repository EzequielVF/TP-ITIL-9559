namespace TP_ITIL_9559.Model
{
    public class IncidentMetricsDto
    {
        public float[] IncidentsPerDay {get;set;}
        public float[] IncidentsPerHour {get;set;}
        public int HourWithMostIncidents {get;set;}
        public string DayWithMostIncidents {get;set;}
        public double AvgResolutionTime {get;set;}
        public string mostAfectedItemName { get; set; }
    }
}