using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHome_Backend.Data;
using SmartHome_Backend.Model;
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
        [HttpPost]
        public async Task<ActionResult<Huis>> VoegHuisToe(Huis huis)
        {
            var Gebruiker = await _context.Huizen.FirstOrDefaultAsync(x => x.GebruikersId == huis.GebruikersId);
            if (Gebruiker !=null)
            {
                return BadRequest("De gebruiker heeft al een huis");
            }
            _context.Huizen.Add(huis);
            await _context.SaveChangesAsync();
            return Ok(huis);
        }
    }
}
