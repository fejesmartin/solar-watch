using Microsoft.AspNetCore.Identity;

namespace SolarWatchApp.DataServices.Authentication;

public interface ITokenService
{
    public string CreateToken(IdentityUser user);
}