using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeSpace.Data;

namespace SafeSpace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _context.Doctors.ToListAsync();

            return Ok(doctors);
        }
    }
}