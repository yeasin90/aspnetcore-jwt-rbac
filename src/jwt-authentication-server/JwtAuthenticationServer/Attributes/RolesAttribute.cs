using Microsoft.AspNetCore.Authorization;
using System;

namespace JwtAuthenticationServer.Attributes
{
    public class RolesAttribute : AuthorizeAttribute
    {
        public RolesAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}
