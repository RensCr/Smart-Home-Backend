using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHome_Backend.Data;
using SmartHome_Backend.Entities;
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

    }
}
