using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository
{
    Task<IEnumerable<Dish>> GetAllAsync(Guid restaurantId);

    Task<Dish?> GetByIdAsync(Guid restaurantId, Guid dishId);

    Task<Dish> CreateAsync(Dish dish);

    Task<int> SaveChangesAsync();
}