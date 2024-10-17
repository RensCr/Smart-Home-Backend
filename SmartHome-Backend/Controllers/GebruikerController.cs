using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHome_Backend.Data;
using SmartHome_Backend.Model;
namespace SmartHome_Backend.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class GebruikerController : Controller
    {
        private readonly DataContext _context;
        public GebruikerController(DataContext context )
        {
            _context = context;
        }
        [HttpGet("/GebruikersNaam")]
        public IActionResult GetGebruikersNaam()
        {
            return Ok("GebruikersNaam");
        }

        [HttpGet]
        public async Task<ActionResult<List<Gebruiker>>> AlleGebruikers()
        {
           var Gebruikers = await _context.Gebruikers.ToListAsync();
            return Ok(Gebruikers);
        }
        [HttpPost]
        public async Task<IActionResult> VoegGebruikerToe(Gebruiker gebruiker)
        {
            var Email = await _context.Gebruikers.FirstOrDefaultAsync(x => x.Email == gebruiker.Email);
            if (Email != null)
            {
                return BadRequest("Er bestaat al een account met dit email adres");
            }
            _context.Gebruikers.Add(gebruiker);
            await _context.SaveChangesAsync();
            return Ok(gebruiker);
        }

    }
}
