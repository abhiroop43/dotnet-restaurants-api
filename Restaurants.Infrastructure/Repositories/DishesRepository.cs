using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<IEnumerable<Dish>> GetAllAsync(Guid restaurantId)
    {
        return await dbContext
            .Dishes
            .Where(d => d.RestaurantId == restaurantId 
                        && d.IsActive)
            .ToListAsync();
    }

    public async Task<Dish?> GetByIdAsync(Guid restaurantId, Guid dishId)
    {
        return await dbContext
            .Dishes
            .Where(d => d.RestaurantId == restaurantId 
                        && d.Id == dishId 
                        && d.IsActive)
            .FirstOrDefaultAsync();
    }

    public async Task<Dish> CreateAsync(Dish dish)
    {
        var id = Guid.NewGuid();
        dish.Id = id;
        await dbContext.Dishes.AddAsync(dish);
        return dish;
    }

    public async Task<int> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}