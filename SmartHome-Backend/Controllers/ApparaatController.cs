using Microsoft.AspNetCore.Mvc;
using SmartHome_Backend.Data;
using SmartHome_Backend.Model;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
namespace SmartHome_Backend.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    [Authorize]
    public class ApparaatController : Controller
    {
        private readonly DataContext _context;
        public ApparaatController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("/Apparaten")]
        [Authorize]
        public async Task<ActionResult<List<OntvangApparaten>>> AlleApparaten(int HuisId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim not found.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID claim.");
            }

            var huis = await _context.Huizen.FirstOrDefaultAsync(h => h.Id == HuisId && h.GebruikersId == userId);
            if (huis == null)
            {
                return BadRequest("Invalid HuisId or unauthorized access.");
            }

            var ontvangApparaten = await _context.Apparaten
                .Where(a => a.HuisId == HuisId)
                .Join(_context.ApparatenTypes,
                      apparaat => apparaat.ApparaatTypeId,
                      type => type.Id,
                      (apparaat, type) => new OntvangApparaten
                      {
                          Id = apparaat.Id,
                          Naam = apparaat.Naam,
                          Slim = apparaat.Slim,
                          apparaatType = type.Type,
                          HuisId = apparaat.HuisId
                      })
                .ToListAsync();

            return Ok(ontvangApparaten);
        }


        [HttpPost]
        [Authorize]
        [SwaggerResponse(StatusCodes.Status409Conflict, "HuisId of ApparaatTypeId bestaat niet.")]
        public async Task<ActionResult> NieuwApparaat([FromBody] ApparaatToevoegen apparaat)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim not found.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID claim.");
            }
            var huis = await _context.Huizen.FirstOrDefaultAsync(h => h.Id == apparaat.HuisId && h.GebruikersId == userId);
            if (huis == null)
            {
                return BadRequest("Invalid HuisId or unauthorized access.");
            }
            var nieuwApparaat = new Apparaat
            {
                Naam = apparaat.Naam,
                ApparaatTypeId = apparaat.ApparaatTypeId,
                HuisId = apparaat.HuisId,
                Slim = apparaat.Slim
            };
            _context.Apparaten.Add(nieuwApparaat);
            await _context.SaveChangesAsync();
            return Ok(apparaat);
        }
    }
}
