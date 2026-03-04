using System.ComponentModel.DataAnnotations;

namespace SafeSpace.Models
{
    public class Patient : BaseUser
    {
        [Required]
        public int Age { get; set; }

        [Required]
        public required string Gender { get; set; }
        public bool IsEmailVerified { get; set; } = false;

        public string? EmailVerificationToken { get; set; }
    }
}