using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Restaurants.Application.Users.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Authorization;

public class TokenProvider(IConfiguration configuration)
{
  public LoginResponse Create(User user, IList<string> roles)
  {
    var secretKey = configuration.GetValue<string>("Jwt:Secret")!;
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha384);
    var expiry =
        new DateTimeOffset(DateTime.Now.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationTimeInMinutes")));

    List<Claim> claims = new List<Claim>()
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id),
      new Claim(JwtRegisteredClaimNames.Email, user.Email!),
    };

    if (user.DateOfBirth != null)
    {
      claims.Add(new Claim("DateOfBirth", user.DateOfBirth.ToString()!));
    }

    if (!string.IsNullOrEmpty(user.Nationality))
    {
      claims.Add(new Claim("Nationality", user.Nationality!));
    }

    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
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

  public string GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }
}
