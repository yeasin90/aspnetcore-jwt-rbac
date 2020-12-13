using JwtAuthenticationServer.Attributes;
using JwtAuthenticationServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace JwtAuthenticationServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpContextAccessor _authContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpContextAccessor authContext)
        {
            _logger = logger;
            _authContext = authContext;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
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
    }
}
