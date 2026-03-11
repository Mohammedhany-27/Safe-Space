using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
        private readonly SafeSpace.Services.EmailService _emailService;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context,
                      SafeSpace.Services.EmailService emailService,
                      IConfiguration config)
        {
            _context = context;
            _emailService = emailService;
            _config = config;
        }

        // Register Patient
        [HttpPost("register")]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.Patients.Any(p => p.Email == model.Email))
                return BadRequest("Email already exists");

            var token = Guid.NewGuid().ToString();

            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Passwords do not match");
            }

            var patient = new Patient
            {
                FullName = model.FullName,
                DisplayName = model.DisplayName,
                Email = model.Email,
                PasswordHash = model.Password,
                Age = model.Age,
                Gender = model.Gender,
                EmailVerificationToken = token,
                IsEmailVerified = false
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            
            var verificationLink =
                $"{Request.Scheme}://{Request.Host}/api/auth/verify-email?token={token}";

         
            _emailService.SendVerificationEmail(model.Email, verificationLink);

            return Ok("Account created. Please verify your email.");
        }

        // Verify Email
        [HttpGet("verify-email")]
        public IActionResult VerifyEmail(string token)
        {
            var user = _context.Patients
                .FirstOrDefault(p => p.EmailVerificationToken == token);

            if (user == null)
                return BadRequest("Invalid token");

            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;

            _context.SaveChanges();


            return Ok("Email verified successfully");
        }

        // Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _context.Patients
                .FirstOrDefault(p => p.Email == model.Email);

            if (user == null)
                return Unauthorized("Invalid Email");

            if (!user.IsEmailVerified)
                return Unauthorized("Please verify your email first");

            if (user.PasswordHash != model.Password)
                return Unauthorized("Invalid Password");

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.FullName)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = jwt,
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.Email
                }
            });
        }
    }
    }