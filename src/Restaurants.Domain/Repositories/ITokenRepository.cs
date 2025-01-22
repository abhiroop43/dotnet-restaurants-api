namespace Restaurants.Domain.Repositories;

public interface ITokenRepository
{
  Task<UserRefreshToken> AddRefreshTokenAsync(UserRefreshToken userRefreshToken);
  bool DeleteRefreshTokenForUser(string userId);
  UserRefreshToken? GetRefreshTokenForUser(string userId);
  Task<int> SaveChangesAsync();

}
