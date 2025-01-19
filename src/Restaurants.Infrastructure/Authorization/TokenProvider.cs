using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Restaurants.Application.Users.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Authorization;

public class TokenProvider(IConfiguration configuration)
{
  public LoginResponse Create(User user)
  {
    var secretKey = configuration.GetValue<string>("Jwt:Secret")!;
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha384);
    var expiry =
        new DateTimeOffset(DateTime.Now.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationTimeInMinutes")));

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("DateOfBirth", user.DateOfBirth.ToString()!)
        ]),
      // Expires = DateTime.Now.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationTimeInMinutes")),
      Expires = expiry.DateTime,
      SigningCredentials = credentials,
      Issuer = configuration.GetValue<string>("Jwt:Issuer"),
      Audience = configuration.GetValue<string>("Jwt:Audience")
    };

    var handler = new JsonWebTokenHandler();
    var token = handler.CreateToken(tokenDescriptor);

    return new LoginResponse
    {
      Token = token,
      Expires = expiry.DateTime
    };
  }
}
