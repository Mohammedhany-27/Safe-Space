using Microsoft.AspNetCore.Http;

namespace SafeSpace.DTOs
{
    public class UpdatePatientDto
    {
        public string? DisplayName { get; set; }

        public int? Age { get; set; }

        public string? Gender { get; set; }

        public IFormFile? Image { get; set; }
    }
}