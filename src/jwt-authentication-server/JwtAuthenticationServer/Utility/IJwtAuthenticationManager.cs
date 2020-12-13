using JwtAuthenticationServer.Models;

namespace JwtAuthenticationServer.Utility
{
    public interface IJwtAuthenticationManager
    {
        string GenerateJwtToken(UserModel user);
    }
}
