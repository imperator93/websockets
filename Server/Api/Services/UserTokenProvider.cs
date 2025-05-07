using System.Security.Claims;
using System.Text;
using Api.Dto;
using Api.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class UserTokenProvider(IConfiguration configuration)
{
    public string Create(UserResponse userResponse)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Name, userResponse.Name)
            ]),
            Issuer = configuration["JwtSettings:Issuer"]!,
            Audience = configuration["JwtSettings:Audience"]!,
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(configuration.GetValue<int>("JwtSettings:ExpirationTime"))
        };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }
}