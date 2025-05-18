namespace TP_ITIL_9559.Model
{
    public class ChangeDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int ConfigurationItemId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string? State { get; set; }
        public int AssignedUserId { get; set; }
        public string Impact { get; set; }
        public string Priority { get; set; }
        public string? ScheduledDate { get; set; }
        public List<int> IncidentIds { get; set; }
        public List<int> ProblemIds { get; set; }

        public ChangeDto()
        {
            IncidentIds = new List<int>();
            ProblemIds = new List<int>();
        }
    }
}
