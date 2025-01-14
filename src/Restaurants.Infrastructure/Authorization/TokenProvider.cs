using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Authorization;

public class TokenProvider(IConfiguration configuration)
{
    public string Create(User user)
    {
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET")!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("DateofBirth", user.DateOfBirth.ToString()!)
            ]),
            Expires = DateTime.Now.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationTimeInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration.GetValue<string>("Jwt:Issuer"),
            Audience = configuration.GetValue<string>("Jwt:Audience")
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}