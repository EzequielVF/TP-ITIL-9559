namespace TP_ITIL_9559.Data.Domain 
{
    public class ConfigurationItem : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public virtual User User {get; set;}
        public string VersionId {get; set;}
        public string VersionHistory {get; set;}
        public bool Disabled { get; set; }
    }
}