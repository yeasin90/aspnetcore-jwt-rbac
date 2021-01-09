using JwtAuthenticationServer.Authorizations;
using JwtAuthenticationServer.Authorizations.Policies;
using JwtAuthenticationServer.Extensions;
using JwtAuthenticationServer.Policies.Authorizations;
using JwtAuthenticationServer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace JwtAuthenticationServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Added for using IHttpContextAccessor from controller
            services.AddHttpContextAccessor();

            // Added for CORS configuration
            services.ConfigureCors();

            // Added for JWT configuration
            services.ConfigureJwt(Configuration);

            services.AddAuthorization(options =>
            {
                // Custom authorization policy
                // https://devblogs.microsoft.com/aspnet/jwt-validation-and-authorization-in-asp-net-core/
                options.AddPolicy(KeyConstants.CustomAuthorizationPolicyName, policy => policy.Requirements.Add(new OfficeNumberRequirement(10)));

                // Another custom authorzation policy added in authorization middleware 
                // https://www.youtube.com/watch?v=RBMO_hruKaI&list=PLOeFnOV9YBa7dnrjpOG6lMpcyd7Wn7E8V&index=4
                options.AddPolicy(ClaimTypes.DateOfBirth, policy =>
                {
                    policy.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            // Default from ASP.NET Core 5
            services.AddControllers();

            // DI registration
            services.AddSingleton<IJwtAuthenticationManager, JwtAuthenticationManager>();
            services.AddSingleton<IAuthorizationHandler, OfficeNumberPolicyHandler>();
            services.AddSingleton<IAuthorizationHandler, CustomRequireClaimHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // For enabling services.ConfigureCors(); from ConfigurationService.
            // This must be between app.UseRouting(); and app.UseEndpoints
            app.UseCors();

            // For enabling services.ConfigureJwt(Configuration); from ConfigurationService.
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Server is up and running!");
                });
            });
        }
    }
}
