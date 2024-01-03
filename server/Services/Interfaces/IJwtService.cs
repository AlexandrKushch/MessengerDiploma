using System.Security.Claims;
using Server.Data.Models;

namespace Server.Services.Interfaces
{
    public interface IJwtService
    {
        Task<List<Claim>> GenerateClaimsAsync(User user);

        Task<string> GenerateTokenAsync(User user);
    }
}