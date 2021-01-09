using JwtAuthenticationServer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationServer.Utility
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly IConfiguration _configuration;

        public JwtAuthenticationManager(IConfiguration configurarion)
        {
            _configuration = configurarion;
        }

        /// <summary>
        /// Create a Jwt token from client's username and password
        /// To know more, 
        /// link : https://www.youtube.com/watch?v=vlRqxCpCKGU
        /// link : https://www.youtube.com/watch?v=vWkPdurauaA&t=790s
        /// link : https://code-maze.com/authentication-aspnetcore-jwt-1/
        /// </summary>
        public string GenerateJwtToken(UserModel user)
        {
            // 1. generate auth key using a custom secret defined in server
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            // 2. create list of named claims to add into jwt token for this user
            // Types of named claims : 'ClaimTypes', 'JwtRegisteredClaimNames' or any custom value
            // For list of jwt claim names, follow : https://www.tpisoftware.com/tpu/articleDetails/1864
            var claims = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Anything1", "Custom value"),
                new Claim(ClaimTypes.DateOfBirth, "TempDoB")
            });

            // 2. create token description
            var tokenDescription = new SecurityTokenDescriptor()
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.Now.AddMinutes(3),
                SigningCredentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256),
                Subject = claims
            };

            // 3. create token handler instance
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
