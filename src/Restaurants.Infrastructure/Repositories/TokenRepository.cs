using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class TokenRepository(RestaurantsDbContext dbContext) : ITokenRepository
{

  public async Task<UserRefreshToken> AddRefreshTokenAsync(UserRefreshToken userRefreshToken)
  {
    userRefreshToken.Id = Guid.NewGuid();
    userRefreshToken.CreatedAt = DateTime.Now;
    await dbContext.AddAsync(userRefreshToken);
    return userRefreshToken;
  }

  public bool DeleteRefreshTokenForUserAsync(string userId)
  {
    dbContext.UserRefreshTokens.RemoveRange(
      dbContext.UserRefreshTokens.Where(t => t.UserId == userId));

    return true;
  }

  public UserRefreshToken? GetRefreshTokenForUser(string userId)
  {
    return dbContext.UserRefreshTokens
      .Where(token => token.UserId == userId)
      .OrderByDescending(token => token.CreatedAt)
      .FirstOrDefault();
  }


  public async Task<int> SaveChangesAsync()
  {
    return await dbContext.SaveChangesAsync();
  }
}
