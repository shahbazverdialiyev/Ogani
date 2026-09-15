using Ogani.WebApp.Business.DTOs.Auth;

namespace Ogani.WebApp.Business.Authentication
{
    public interface IJwtService
    {
        string GenerateToken(AppUserDTO user);
    }
}