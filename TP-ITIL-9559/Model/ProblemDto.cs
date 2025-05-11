namespace TP_ITIL_9559.Model
{
    public class ProblemDto
    {
        public string title { get; set; }
        public string description { get; set; }
        public int userId { get; set; }
        public int configurationItemId { get; set; }
        public int assignedUserId { get; set; }
        public string impact { get; set; }
        public string priority { get; set; }
        public List<int> incidentIds { get; set; }

        public ProblemDto()
        {
            incidentIds = new List<int>();
        }
    }
}
