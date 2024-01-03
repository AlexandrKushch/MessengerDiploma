using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Server.Data.Models;
using Server.Data.Models.Configuration;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<User> _userManager;

        public JwtService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<Claim>> GenerateClaimsAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var claims = await GenerateClaimsAsync(user);

            var jwt = new JwtSecurityToken
            (
                issuer: JwtConfiguration.ISSUER,
                audience: JwtConfiguration.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: new SigningCredentials(JwtConfiguration.SIGNING_KEY, SecurityAlgorithms.HmacSha256)
            );
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}