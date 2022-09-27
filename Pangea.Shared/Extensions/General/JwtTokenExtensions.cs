using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Pangea.Shared.Extensions.General
{
    public static class JwtTokenExtensions
    {
        private static JwtSecurityToken? GetDecodedToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

            return jwtToken;
        }

        public static int ExtractUserId(this string jwtToken)
        {
            var token = GetDecodedToken(jwtToken);
            var userid = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return Convert.ToInt32(userid);
        }

        public static string? ExtractUsername(this string jwtToken)
        {
            var token = GetDecodedToken(jwtToken);
            var username = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            return username;
        }

        public static string? ExtractClaim(this string jwtToken, string claimType)
        {
            var token = GetDecodedToken(jwtToken);
            var value = token?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

            return value;
        }

        public static IEnumerable<Claim> GetGlaims(this string jwtToken)
        {
            var token = GetDecodedToken(jwtToken);

            return token!.Claims;
        }
    }
}
