using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace JwtAuthenticationServer.Extensions
{
    /// <summary>
    /// In this class, add your custom configurations for Stratup.cs->ConfigureServices middleware 
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// To know more on CORS for ASP.NET Core, 
        /// : https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-5.0
        /// : https://code-maze.com/net-core-web-development-part2/
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                // Use 'AddPolicy' to add named CORS policty
                // https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-5.0
                options.AddDefaultPolicy(
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                // There are many more properties avaialble.
                // To know actual usage of some of them (in relation to what is used in this code),
                // (very good article): https://devblogs.microsoft.com/aspnet/jwt-validation-and-authorization-in-asp-net-core/ 
                // : https://code-maze.com/authentication-aspnetcore-jwt-1/
                // : https://www.tpisoftware.com/tpu/articleDetails/1864
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidAudience = configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"])),
                    // ClockSkew = expiration time of a token. Default is 5-minutes.
                    // If you set 'new SecurityTokenDescriptor().Expires = 1min', then total token expiration time will be 6-minutes
                    // Set ClockSkew = TimeSpan.Zero to ensure only 'new SecurityTokenDescriptor().Expires = 1min' is used
                    // link : https://stackoverflow.com/questions/43045035/jwt-token-authentication-expired-tokens-still-working-net-core-web-api
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
