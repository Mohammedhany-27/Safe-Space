using System.ComponentModel.DataAnnotations;

namespace SafeSpace.Models
{
    public class Doctor : BaseUser
    {
        [Required]
        public required string Specialization { get; set; }

        [Required]
        public required string Bio { get; set; }
    }
}