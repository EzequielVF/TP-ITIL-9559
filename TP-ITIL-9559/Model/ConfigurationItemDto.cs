namespace TP_ITIL_9559.Model
{
    public class ConfigurationItemDto
    {
        public string title { get; set; }
        public string description { get; set; }
        public int userId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string versionId { get; set; }
        public string? versionHistory { get; set; }
    }
}
