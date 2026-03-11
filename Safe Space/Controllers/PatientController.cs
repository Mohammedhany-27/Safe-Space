using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeSpace.Data;
using SafeSpace.DTOs;
using System.Security.Claims;

namespace SafeSpace.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PatientController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == userId);

            if (patient == null)
                return NotFound();

            var dto = new UserProfileDto
            {
                FullName = patient.FullName,
                Email = patient.Email,
                DisplayName = patient.DisplayName,
                Age = patient.Age,
                Gender = patient.Gender,
                ImageUrl = patient.ProfileImageUrl ?? "/images/default.png"
            };

            return Ok(dto);
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null)
                return BadRequest("No image");

            var extension = Path.GetExtension(image.FileName);

            var fileName = Guid.NewGuid() + extension;

            var folder = Path.Combine(_environment.WebRootPath, "images");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var patient = await _context.Patients.FindAsync(userId);

            patient.ProfileImageUrl = "/images/" + fileName;

            await _context.SaveChangesAsync();

            return Ok(patient.ProfileImageUrl);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdatePatientDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var patient = await _context.Patients.FindAsync(userId);

            if (patient == null)
                return NotFound();

            if (!string.IsNullOrEmpty(dto.DisplayName))
                patient.DisplayName = dto.DisplayName;

            if (dto.Age.HasValue)
                patient.Age = dto.Age.Value;

            if (!string.IsNullOrEmpty(dto.Gender))
                patient.Gender = dto.Gender;

            await _context.SaveChangesAsync();

            return Ok("Profile updated");
        }
    }
}