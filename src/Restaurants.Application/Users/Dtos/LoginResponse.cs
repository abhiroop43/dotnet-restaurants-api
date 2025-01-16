namespace Restaurants.Application.Users.Dtos;

public class LoginResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public long Expires { get; set; }
}