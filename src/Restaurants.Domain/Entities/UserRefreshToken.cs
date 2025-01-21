public class UserRefreshToken
{
  public Guid Id { get; set; }
  public string RefreshToken { get; set; } = default!;
  public DateTime RefreshTokenExpiry { get; set; }
  public string UserId { get; set; } = default!;

}
