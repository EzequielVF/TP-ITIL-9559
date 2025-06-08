using System.ComponentModel.DataAnnotations;

namespace TP_ITIL_9559.Data.Domain
{
    public class Client : EntityBase
    {
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}
