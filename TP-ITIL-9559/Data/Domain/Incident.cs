using System.ComponentModel.DataAnnotations.Schema;

namespace TP_ITIL_9559.Data.Domain 
{
    public class Incident : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User {get; set;}
        public int? ConfigurationItemId {get; set;}
        [ForeignKey("ConfigurationItemId")]
        public virtual ConfigurationItem ConfigurationItem {get; set;}
        public int TrackingNumber {get;set;}
        public DateTime ClosedDate {get;set;}
        public DateTime LastUpdated {get;set;}
        public string? RootCause {get;set;}
        public string ClientName {get;set;}
        public string ClientEmail {get;set;}
        public string State {get;set;}
        public int AssignedUserId {get;set;}
        [ForeignKey("AssignedUserId")]
        public virtual User AssignedUser {get;set;}
        public string Impact {get;set;}
        public string Priority {get;set;}
        public List<string> Comments {get;set;}

        public Incident(){
            Comments = new List<string>();
        }

        public virtual ICollection<Problem> Problems { get; set; }
        public virtual ICollection<Change> Changes { get; set; }
    }
}