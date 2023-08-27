using System.Security.Claims;

namespace Ploomers_Project_API.TokenService
{
    public interface ITokenService
    {
        string GenerateAcessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
