using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthenticationServer.Authorizations.Policies
{
    // https://www.youtube.com/watch?v=RBMO_hruKaI&list=PLOeFnOV9YBa7dnrjpOG6lMpcyd7Wn7E8V&index=4
    public class CustomClaimPolicy : IAuthorizationRequirement
    {
        public CustomClaimPolicy(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }

    public class CustomRequireClaimHandler : AuthorizationHandler<CustomClaimPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomClaimPolicy requirement)
        {
            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);

            if(hasClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
