namespace Restaurants.Application.Users.Dtos;

public class RegisterRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
}