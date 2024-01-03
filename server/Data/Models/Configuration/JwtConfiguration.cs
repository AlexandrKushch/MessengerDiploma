using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.Data.Models.Configuration
{
    public class JwtConfiguration
    {
        public const string ISSUER = "TF";
        public const string AUDIENCE = "localhost:port";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("security key for me"));
    }
}