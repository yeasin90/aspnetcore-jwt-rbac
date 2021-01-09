using JwtAuthenticationServer.Attributes;
using JwtAuthenticationServer.Authorizations;
using JwtAuthenticationServer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace JwtAuthenticationServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : Controller
    {
        private readonly IHttpContextAccessor _authContext;

        public InventoryController(IHttpContextAccessor authContext)
        {
            _authContext = authContext;
        }

        [HttpGet, Route("salaries")]
        [Roles(UserRoles.Manager)]
        public IActionResult GetSalaries()
        {
            var current = _authContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return Ok($"Current user : {current}. Accesible only by Manager. Sales are not allowed");
        }

        [HttpGet, Route("stock")]
        [Roles(UserRoles.Manager, UserRoles.Sales)]
        public IActionResult GetStock()
        {
            return Ok("Accesible by both Manager and Sales.");
        }

        [HttpGet, Route("address")]
        [AllowAnonymous]
        public IActionResult GetAddress()
        {
            return Ok("Accesible by Everyone, no jwt token required.");
        }

        [HttpGet, Route("warehouse")]
        [Authorize(Policy = KeyConstants.CustomAuthorizationPolicyName)]
        public IActionResult GetWarehouse()
        {
            return Ok("Policy based access. Only sales can see.");
        }

        [Authorize(Policy = ClaimTypes.DateOfBirth)]
        public IActionResult SecretPolicy()
        {
            return Ok($"Request had {ClaimTypes.DateOfBirth}. Response OK");
        }
    }
}
