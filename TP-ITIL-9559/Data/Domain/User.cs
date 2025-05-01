using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_ITIL_9559.Data.Domain
{
    public class User : EntityBase
    {
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Incident> Incidents { get; set; }

        [InverseProperty("AssignedUser")]
        public virtual ICollection<Incident> AssignedIncidents { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Problem> Problems { get; set; }

        [InverseProperty("AssignedUser")]
        public virtual ICollection<Problem> AssignedProblems { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Change> Changes { get; set; }

        [InverseProperty("AssignedUser")]
        public virtual ICollection<Change> AssignedChanges { get; set; }

        public virtual ICollection<ConfigurationItem> Items { get; set; }
    }
}
