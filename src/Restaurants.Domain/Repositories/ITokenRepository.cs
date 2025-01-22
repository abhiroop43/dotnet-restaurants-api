namespace Restaurants.Domain.Repositories;

public interface ITokenRepository
{
  Task<UserRefreshToken> AddRefreshTokenAsync(UserRefreshToken userRefreshToken);
  bool DeleteRefreshTokenForUserAsync(string userId);
  UserRefreshToken? GetRefreshTokenForUser(string userId);
}
