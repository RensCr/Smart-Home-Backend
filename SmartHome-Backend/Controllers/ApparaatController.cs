using Microsoft.AspNetCore.Mvc;
using SmartHome_Backend.Data;
using SmartHome_Backend.Model;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
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
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status409Conflict, "HuisId of ApparaatTypeId bestaat niet.")]
        public async Task<ActionResult<Apparaat>> NieuwApparaat(Apparaat apparaat)
        {
            var huisBestaat = await _context.Huizen.AnyAsync(h => h.Id == apparaat.HuisId);
            if (!huisBestaat)
            {
                return Conflict("Test");
            }

            var apparaatTypeBestaat = await _context.ApparatenTypes.AnyAsync(at => at.Id == apparaat.ApparaatTypeId);
            if (!apparaatTypeBestaat)
            {
                return BadRequest("ApparaatTypeId bestaat niet.");
            }

            _context.Apparaten.Add(apparaat);
            await _context.SaveChangesAsync();
            return Ok(apparaat);
        }


    }
}
