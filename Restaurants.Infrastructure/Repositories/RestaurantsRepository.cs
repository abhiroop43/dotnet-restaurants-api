using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await dbContext.Restaurants
            .Where(r => r.IsActive)
            // .Include(r => r.Dishes)
            .ToListAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetAllByOwnerAsync(string ownerId)
    {
        return await dbContext.Restaurants
            .Where(r => r.IsActive
                        && r.OwnerId.ToUpper() == ownerId.ToUpper())
            .ToListAsync();
    }

    public async Task<Restaurant?> GetByIdAsync(Guid id)
    {
        // var guid = Guid.Parse(id);
        return await dbContext.Restaurants
            .Where(r => r.IsActive)
            .Include(r => r.Dishes.Where(d => d.IsActive))
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Restaurant> CreateAsync(Restaurant restaurant)
    {
        var id = Guid.NewGuid();
        restaurant.Id = id;
        await dbContext.Restaurants.AddAsync(restaurant);
        return restaurant;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Restaurant>, int)> GetByPhraseAsync(string? searchPhrase, int pageSize,
        int pageNumber)
    {
        var searchPhraseLower = searchPhrase?.ToLower();

        var baseQuery = dbContext.Restaurants
            .Where(r => r.IsActive
                        && (searchPhraseLower == null || r.Name.ToLower().Contains(searchPhraseLower) ||
                            r.Description.ToLower().Contains(searchPhraseLower))
            );

        var totalCount = await baseQuery.CountAsync();

        var restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (restaurants, totalCount);
    }
}