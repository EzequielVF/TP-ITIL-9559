namespace TP_ITIL_9559.Model 
{
    public class IncidentDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int ConfigurationItemId { get; set; }
        public string? RootCause {get;set;}
        public string ClientName {get;set;}
        public string ClientEmail {get;set;}
        public string? State {get;set;}
        public int AssignedUserId {get;set;}
        public string Impact {get;set;}
        public string Priority {get;set;}
    }
}