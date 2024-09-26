using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHome_Backend.Data;
using SmartHome_Backend.Entities;
namespace SmartHome_Backend.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class HuisController : Controller
    {
        private readonly DataContext _context;
        public HuisController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Huis>>> AlleHuizen()
        {
            var Huizen = await _context.Huizen.ToListAsync();
            return Ok(Huizen);
        }
    }
}
