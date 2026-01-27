using FinanceTrackerApi.Entities.Login;
using System.Security.Claims;

namespace FinanceTrackerApi.Services.TokenService;
public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
