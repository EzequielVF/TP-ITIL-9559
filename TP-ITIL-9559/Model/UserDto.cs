using System.ComponentModel.DataAnnotations;

namespace TP_ITIL_9559.Model
{
    public class UserDto
    {        
        [Required]
        public string email {get; set;}

        [Required]
        public string password {get; set;}
    }
}