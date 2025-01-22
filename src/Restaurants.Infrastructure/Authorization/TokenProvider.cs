using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Restaurants.Application.Users.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization;

public class TokenProvider(IConfiguration configuration, ITokenRepository tokenRepository)
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
    var refreshToken = GenerateRefreshToken();
    var refreshTokenExpires = DateTime.Now.AddDays(configuration.GetValue<int>("Jwt:RefreshTokenValidityInDays"));

    tokenRepository.AddRefreshTokenAsync(new UserRefreshToken
    {
      UserId = user.Id,
      RefreshToken = refreshToken,
      RefreshTokenExpiry = refreshTokenExpires
    });

    return new LoginResponse
    {
      Token = token,
      Expires = expiry.DateTime,
      RefreshToken = refreshToken,
      RefreshTokenExpires = refreshTokenExpires
    };
  }

  public string GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  public bool ValidateRefreshToken(string userId, string refreshToken)
  {
    var storedToken = tokenRepository.GetRefreshTokenForUser(userId);

    if (storedToken == null)
    {
      return false;
    }

    if (storedToken.RefreshToken != refreshToken)
    {
      return false;
    }

    if (storedToken.RefreshTokenExpiry < DateTime.Now)
    {
      return false;
    }

    return true;
  }

  /*public async ClaimsPrincipal? GetClaimsPrincipalFromToken(string token)*/
  /*{*/
  /*  var tokenValidationParameters = new TokenValidationParameters*/
  /*  {*/
  /*    ValidateIssuer = false,*/
  /*    ValidateAudience = false,*/
  /*    ValidateLifetime = false,*/
  /*    ValidateIssuerSigningKey = true,*/
  /*    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Secret")))*/
  /*  };*/
  /**/
  /*  var tokenHandler = new JsonWebTokenHandler();*/
  /*  var validationResult = await tokenHandler*/
  /*    .ValidateTokenAsync(token, tokenValidationParameters);*/
  /**/
  /*  var securityToken = validationResult.SecurityToken;*/
  /**/
  /*  if (securityToken is not JsonWebToken jwtToken || !jwtToken.Alg.Equals(SecurityAlgorithms.HmacSha384, StringComparison.InvariantCultureIgnoreCase))*/
  /*  {*/
  /*    return null;*/
  /*  }*/
  /**/
  /*  return principal;*/
  /*}*/
}
