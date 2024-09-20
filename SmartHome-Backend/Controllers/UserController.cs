using Microsoft.AspNetCore.Mvc;
using SmartHome_Backend.Repositories;
namespace SmartHome_Backend.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UserController : Controller
    {
        private UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("/GebruikersNaam")]
        public IActionResult GetGebruikersNaam()
        {
            return Ok("GebruikersNaam");
        }

    }
}
