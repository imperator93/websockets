using System.Security.Claims;
using System.Text;
using Api.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class TokenProvider
{
    public string Create(User user, JwtSettings jwtSettings)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            ]),
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddHours(jwtSettings.ExpirationTime),
        };

        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }
}