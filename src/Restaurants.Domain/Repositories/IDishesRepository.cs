using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository
{
    Task<IEnumerable<Dish>> GetAllAsync(Guid restaurantId);

    Task<Dish?> GetByIdAsync(Guid restaurantId, Guid dishId);

    Task<Dish> CreateAsync(Dish dish);

    Task<bool> CheckIfDishExistsAndIsActive(Guid restaurantId, string dishName);

    Task<int> SaveChangesAsync();
}