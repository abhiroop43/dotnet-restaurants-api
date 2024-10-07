using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository repository, ILogger<RestaurantsService> logger)
    : IRestaurantsService
{
    public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
    {
        logger.LogInformation("Getting all restaurants");
        return await repository.GetAllAsync();
    }
}