using JwtAuthenticationServer.Models;
using JwtAuthenticationServer.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JwtAuthenticationServer.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthManager;
        public AuthController(IJwtAuthenticationManager jwtAuthManager)
        {
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user == null)
                return BadRequest("Invalid client request");

            var requestedUser = DummyUserGenerator.GetDummyUsers()
                .FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);

            if (requestedUser != null)
            {
                var jwtToken = _jwtAuthManager.GenerateJwtToken(requestedUser);
                return Ok(jwtToken);
            }

            return Unauthorized();
        }
    }
}
