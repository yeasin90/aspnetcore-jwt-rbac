using JwtAuthenticationServer.Models;
using JwtAuthenticationServer.Utility;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Login([FromBody]LoginModel user)
        {
            if (user == null)
                return BadRequest("Invalid client request");

            // dummy username and password
            if(user.Username == "1234" && user.Password == "1234")
            {
                var jwtToken = _jwtAuthManager.GenerateJwtToken();
                return Ok(jwtToken);
            }

            return Unauthorized();
        }
    }
}
