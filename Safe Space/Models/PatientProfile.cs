using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeSpace.Models
{
    public class PatientProfile
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = "";

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public int Age { get; set; }
        public string Gender { get; set; } = "";
    }
}
