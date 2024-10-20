using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();

    Task<Restaurant?> GetByIdAsync(Guid id);
    Task<Restaurant> CreateAsync(Restaurant restaurant);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> UpdateAsync(Restaurant restaurant);

    Task<int> SaveChangesAsync();
}