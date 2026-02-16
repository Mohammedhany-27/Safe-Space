using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeSpace.Data;
using SafeSpace.Models;

namespace SafeSpace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // ==========================
        // Register Doctor
        // ==========================
        [HttpPost("register-doctor")]
        public async Task<IActionResult> RegisterDoctor(
            string fullName,
            string email,
            string password,
            string specialization,
            string bio)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                Role = "Doctor"
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var doctor = new DoctorProfile
            {
                UserId = user.Id,
                Specialization = specialization,
                Bio = bio
            };

            _context.DoctorProfiles.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok("Doctor Registered Successfully");
        }

        // ==========================
        // Register Patient
        // ==========================
        [HttpPost("register-patient")]
        public async Task<IActionResult> RegisterPatient(
            string fullName,
            string email,
            string password,
            int age,
            string gender)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                Role = "Patient"
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var patient = new PatientProfile
            {
                UserId = user.Id,
                Age = age,
                Gender = gender
            };

            _context.PatientProfiles.Add(patient);
            await _context.SaveChangesAsync();

            return Ok("Patient Registered Successfully");
        }

        // ==========================
        // Login
        // ==========================
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(
                email,
                password,
                false,
                false);

            if (!result.Succeeded)
                return Unauthorized("Invalid Email or Password");

            return Ok("Login Successful");
        }
    }
}
