using Microsoft.Extensions.Logging;
using Restaurants.Application.Exceptions;
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

    public async Task<Restaurant> GetRestaurantByIdAsync(string id)
    {
        logger.LogInformation($"Getting restaurant with id: {id}");
        var restaurant = await repository.GetByIdAsync(id);

        if (restaurant == null) throw new NotFoundException("This restaurant does not exist");

        return restaurant;
    }
}