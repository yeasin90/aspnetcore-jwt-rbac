using JwtAuthenticationServer.Authorizations;
using JwtAuthenticationServer.Models;
using System.Collections.Generic;

namespace JwtAuthenticationServer.Utility
{
    public static class DummyUserGenerator
    {
        public static List<UserModel> GetDummyUsers() =>
            new List<UserModel>()
            {
                new UserModel(){Username = "manager", Password = "manager", Role = UserRoles.Manager},
                new UserModel(){Username = "sales", Password = "sales", Role = UserRoles.Sales}
            };
    }
}
