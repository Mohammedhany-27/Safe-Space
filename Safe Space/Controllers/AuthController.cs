using Microsoft.AspNetCore.Mvc;
using SafeSpace.Data;
using SafeSpace.Models;
using SafeSpace.DTOs;

namespace SafeSpace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // Register Doctor
        [HttpPost("register-doctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = model.Password, // هنعدلها بعدين لهاش
                Specialization = model.Specialization,
                Bio = model.Bio
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok("Doctor Registered Successfully");
        }

        // Register Patient
        [HttpPost("register-patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = model.Password, // هنعدلها لهاش
                Age = model.Age,
                Gender = model.Gender
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return Ok("Patient Registered Successfully");
        }

        // Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doctor = _context.Doctors.FirstOrDefault(d => d.Email == model.Email);
            var patient = _context.Patients.FirstOrDefault(p => p.Email == model.Email);

            BaseUser? user = null;

            if (doctor != null)
                user = doctor;
            else if (patient != null)
                user = patient;

            if (user == null)
                return Unauthorized("Invalid Email");

            if (user.PasswordHash != model.Password)
                return Unauthorized("Invalid Password");

            return Ok(new
            {
                user.Id,
                user.FullName,
                user.Email
            });
        }
    }
}