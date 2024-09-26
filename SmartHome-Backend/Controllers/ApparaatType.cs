using Microsoft.AspNetCore.Mvc;
using SmartHome_Backend.Data;
using SmartHome_Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace SmartHome_Backend.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ApparaatTypeController : Controller
    {
        private readonly DataContext _context;

        public ApparaatTypeController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApparaatType>>> AlleApparaatTypes()
        {
            var apparaatTypes = await _context.ApparatenTypes.ToListAsync();
            return Ok(apparaatTypes);
        }
    }
}
