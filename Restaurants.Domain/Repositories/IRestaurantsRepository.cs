using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();

    Task<(IEnumerable<Restaurant>, int)> GetByPhraseAsync(string? searchPhrase, int pageSize, int pageNumber);

    Task<IEnumerable<Restaurant>> GetAllByOwnerAsync(string ownerId);

    Task<Restaurant?> GetByIdAsync(Guid id);

    Task<Restaurant> CreateAsync(Restaurant restaurant);

    Task<int> SaveChangesAsync();
}