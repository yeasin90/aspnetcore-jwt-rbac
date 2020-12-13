using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
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
        public string GenerateJwtToken()
        {
            // 1. generate auth key using a custom secret defined in server
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            // 2. create token description
            var tokenDescription = new SecurityTokenDescriptor()
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
            };

            // 3. create token handler instance
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
