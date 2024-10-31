using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        [HttpPost("UpdatePlattegrond")]
        [Authorize]
        public async Task<ActionResult> VoegPlattegrondToe([FromBody] PlattegrondToevoegen Plattegrond)
        {
            var objApparaten = JsonConvert.DeserializeObject(Plattegrond.ApparatenJson);
            string apparatenJson = JsonConvert.SerializeObject(objApparaten);
            var objKamers = JsonConvert.DeserializeObject(Plattegrond.KamersJson);
            string kamersJson = JsonConvert.SerializeObject(objKamers);
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token.");
            }

            int userId = int.Parse(userIdClaim.Value);
            var GebruikerAanHuisId = await _context.Huizen.AnyAsync(h => h.GebruikersId == userId && h.Id == Plattegrond.HuisID);
            if (!GebruikerAanHuisId)
            {
                return Unauthorized("");
            }

            var huis = await _context.Huizen.FindAsync(Plattegrond.HuisID);
            if (huis == null)
            {
                return NotFound("House not found.");
            }

            huis.KamersJson = kamersJson;
            huis.ApparatenJson = apparatenJson;

            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet("/KrijgPlattegrondInformatie")]
        [Authorize]
        public async Task<ActionResult> VerkrijgPlattegrond(int HuisId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token.");
            }

            int userId = int.Parse(userIdClaim.Value);
            var GebruikerAanHuisId = await _context.Huizen.AnyAsync(h => h.GebruikersId == userId && h.Id == HuisId);
            if (!GebruikerAanHuisId)
            {
                return Unauthorized("");
            }
            //get information from db 
            var huis = await _context.Huizen.FindAsync(HuisId);
            if (huis == null)
            {
                return NotFound("House not found.");
            }
            var PlattegrondInformatie = new 
            {
                ApparatenJson = huis.ApparatenJson,
                KamersJson = huis.KamersJson,
            };
            
            return Ok(PlattegrondInformatie);
        }
    }
}
