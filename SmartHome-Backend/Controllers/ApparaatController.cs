using Microsoft.AspNetCore.Mvc;
using SmartHome_Backend.Data;
using SmartHome_Backend.Entities;
using Microsoft.EntityFrameworkCore;
namespace SmartHome_Backend.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ApparaatController : Controller
    {
        private readonly DataContext _context;
        public ApparaatController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Apparaat>>> AlleApparaten()
        {
            var Apparaten = await _context.Apparaten.ToListAsync();
            return Ok(Apparaten);
        }

    }
}
