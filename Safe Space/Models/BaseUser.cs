using System.ComponentModel.DataAnnotations;

namespace SafeSpace.Models
{
    public abstract class BaseUser
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }
    }
}