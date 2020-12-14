using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtAuthenticationServer.Authorizations
{
    // Custom authorization policy : https://devblogs.microsoft.com/aspnet/jwt-validation-and-authorization-in-asp-net-core/
    // A handler that can determine whether a MaximumOfficeNumberRequirement is satisfied
    internal class MaximumOfficeNumberAuthorizationHandler : AuthorizationHandler<MaximumOfficeNumberRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MaximumOfficeNumberRequirement requirement)
        {
            // Bail out if the office number claim isn't present
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == UserRoles.Sales))
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    // A custom authorization requirement which requires office number to be below a certain value
    internal class MaximumOfficeNumberRequirement : IAuthorizationRequirement
    {
        public MaximumOfficeNumberRequirement(int officeNumber)
        {
            MaximumOfficeNumber = officeNumber;
        }

        public int MaximumOfficeNumber { get; private set; }
    }
}
