using Microsoft.AspNetCore.Identity;

namespace AuthService.Services.IServices
{
    public interface ITokenGenerator
    {
        string GenerateToken(IdentityUser user, IEnumerable<string> roles);
    }
}
