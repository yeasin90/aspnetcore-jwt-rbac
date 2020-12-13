﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtAuthenticationServer.Extensions
{
    /// <summary>
    /// In this class, add your custom configurations for Stratup.cs->ConfigureServices middleware 
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// We are using the basic settings for adding CORS policy because for this project allowing any origin, method, 
        /// and header is quite enough. But we can be more restrictive with those settings if we want. 
        /// Instead of the AllowAnyOrigin() method which allows requests from any source, we could use 
        /// the WithOrigins("http://www.something.com") which will allow requests just from the specified source. 
        /// Also, instead of AllowAnyMethod() that allows all HTTP methods, we can use WithMethods("POST", "GET") that 
        /// will allow only specified HTTP methods. Furthermore, we can make the same changes for the AllowAnyHeader() 
        /// method by using, for example, the WithHeaders("accept", "content-type") method to allow only specified headers.
        /// 
        /// To know more, check : https://code-maze.com/net-core-web-development-part2/
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
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
                // There are lot more properties avaialble.
                // To know actual usage of some of them,
                // follow (very good article) : https://devblogs.microsoft.com/aspnet/jwt-validation-and-authorization-in-asp-net-core/ 
                // and also : https://www.tpisoftware.com/tpu/articleDetails/1864
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
                };
            });
        }
    }
}