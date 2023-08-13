using PHKAPI.Models;

namespace PHKAPI.JwtTokens
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(LoginUserDBModel users);
    }
}
