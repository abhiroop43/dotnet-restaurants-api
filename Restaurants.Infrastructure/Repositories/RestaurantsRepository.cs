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
            // .Include(r => r.Dishes)
            .ToListAsync();
    }

    public async Task<Restaurant?> GetByIdAsync(Guid id)
    {
        // var guid = Guid.Parse(id);
        return await dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Restaurant> CreateAsync(Restaurant restaurant)
    {
        var id = Guid.NewGuid();
        restaurant.Id = id;
        // restaurant.Dishes.ForEach(d => d.Id = Guid.NewGuid());
        await dbContext.Restaurants.AddAsync(restaurant);
        var success = await dbContext.SaveChangesAsync();

        if (success > 0) return restaurant;

        throw new Exception("Failed to create restaurant");
    }
}