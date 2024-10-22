using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHome_Backend.Data;
using SmartHome_Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace SmartHome_Backend.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class GebruikerController : Controller
    {
        private readonly DataContext _context;
        public GebruikerController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("/GebruikersNaam")]
        public IActionResult GetGebruikersNaam()
        {
            return Ok("GebruikersNaam");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Gebruiker>>> AlleGebruikers()
        {
            var Gebruikers = await _context.Gebruikers.ToListAsync();
            return Ok(Gebruikers);
        }
        [HttpGet("GebruikersInfo")]
        [Authorize]
        public async Task<ActionResult<Gebruiker>> GetUserInfo()
        {
            // Extract the user ID from the token
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token.");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Fetch the user information from the database
            var gebruiker = await _context.Gebruikers.FirstOrDefaultAsync(x => x.Id == userId);
            if (gebruiker == null)
            {
                return NotFound("User not found.");
            }

            return Ok(gebruiker);
        }

        [HttpPost]
        [Authorize]
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

        [HttpPost("/login")]
        public async Task<ActionResult> Login([FromBody] AccountToegang login)
        {
            var gebruiker = await _context.Gebruikers.FirstOrDefaultAsync(x => x.Email == login.Email);
            if (gebruiker == null)
            {
                return BadRequest("Er bestaat geen account met dit email adres");
            }
            string wachtwoordHash = string.Empty; // TODO: Implementeer wachtwoord hashen
            if (gebruiker.WachtwoordHash != login.Wachtwoord)
            {
                return BadRequest("Wachtwoord is onjuist");
            }

            gebruiker.LaatstIngelogd = DateTime.Now; // Update the LaatstIngelogd field
            await _context.SaveChangesAsync(); // Save changes to the database

            // Generate JWT token
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("dee7c79d56dc4c493b0c57c689a60202eb1eb2339d492bf9bfdfe526275eaa85465248f83489052590ac14d0972dbf504bdcd53c8d69994a06a92705c98b6388fe4a8450ff513d8e00e78045f3c2eda51bbd0e00af4417e4733ae7badac17014cb653209101af6fc7f2c02b2119b53e26d1b7fa8a9580eed23273a39bfa9fda172a1a1c5239bbcf025734246e1e30ce595f6c84a816239f1bcd8214ac8026f26caac740b5a4549a8cb7b12ef2430accb1369b8434b4886c1343e33030fb37bd9521cd8f5b3bd0916d71892ace68ffcfd40089bdbe7940e20ab2cdb3213a2658ca2ca0c3c19b9e1f7e8bf06ee223852eb5aa5a9b6c085fece2f28be0a791774bb"); // Replace with your secret key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", gebruiker.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "Localhost"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        [HttpPost("/Register")]
        public async Task<ActionResult> Aanmaak([FromBody] AccountToegang Aanmaak)
        {
            var gebruikerMail = await _context.Gebruikers.FirstOrDefaultAsync(x => x.Email == Aanmaak.Email);
            if (gebruikerMail != null)
            {
                return BadRequest("Er bestaat al een account met dit email adres");
            }
            var nieuweGebruiker = new Gebruiker
            {
                Naam = Aanmaak.Naam,
                Email = Aanmaak.Email,
                WachtwoordHash = Aanmaak.Wachtwoord,
                WoonPlaats = Aanmaak.WoonPlaats,
                Aangemaakt = DateTime.Now,
                LaatstIngelogd = DateTime.Now
            };
            _context.Gebruikers.Add(nieuweGebruiker);
            await _context.SaveChangesAsync();
            return Ok(nieuweGebruiker);
        }
    }
}
