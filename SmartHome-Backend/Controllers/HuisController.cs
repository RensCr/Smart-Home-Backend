using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<ActionResult<List<Huis>>> AlleHuizen()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token.");
            }

            int userId = int.Parse(userIdClaim.Value);

            var Huizen = await _context.Huizen.Where(h => h.GebruikersId == userId).ToListAsync();
            return Ok(Huizen);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> VoegHuisToe([FromBody]HuisToevoegen huis)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token.");
            }

            int userId = int.Parse(userIdClaim.Value);
            var huisModel = new Huis
            {
                Locatie = huis.Locatie,
                Beschrijving = huis.Beschrijving,
                GebruikersId = userId
            };
            _context.Huizen.Add(huisModel);
            await _context.SaveChangesAsync();
            return Ok(huis);
        }
    }
}
