using Microsoft.IdentityModel.Tokens;
using ShopApp_API_.Entities;
using ShopApp_API_.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopApp_API_.Services.Implementations
{
    public class TokenService : ITokenService
    {
        public string GetToken(string secretKey, string audience, string issuer, AppUser user, IList<string> roles)
        {

            //jwt
            var handler = new JwtSecurityTokenHandler();
            var privateKey = Encoding.UTF8.GetBytes(secretKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256);

            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            ci.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, user.FullName));
            ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            ci.AddClaim(new Claim("Adress", "Baku"));//isdediyimizi bele elave edirik,key value mentigi ile

            ci.AddClaims(roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList());

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddMinutes(30),
                Subject = ci,
                Audience = audience,
                Issuer = issuer
            };

            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}
