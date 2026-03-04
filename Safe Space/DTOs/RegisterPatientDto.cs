using System.ComponentModel.DataAnnotations;

namespace SafeSpace.DTOs
{
    public class RegisterPatientDto
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")]
        public string Password { get; set; }

        public int Age { get; set; }
        public string Gender { get; set; } = "";
    }
}
