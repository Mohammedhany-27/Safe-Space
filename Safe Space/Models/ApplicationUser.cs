using Microsoft.AspNetCore.Identity;

namespace SafeSpace.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = "";
        public string UserType { get; set; } = "";
        public string Role { get; set; }

        public DoctorProfile? DoctorProfile { get; set; }
        public PatientProfile? PatientProfile { get; set; }
    }
}
