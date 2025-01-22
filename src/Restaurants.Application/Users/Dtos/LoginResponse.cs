namespace Restaurants.Application.Users.Dtos;

public class LoginResponse
{
  public string Token { get; set; } = default!;
  public string RefreshToken { get; set; } = default!;
  public DateTime Expires { get; set; }
  public DateTime RefreshTokenExpires { get; set; }
}
