using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository repository, ILogger<RestaurantsService> logger)
    : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await repository.GetAllAsync();

        return restaurants.Select(RestaurantDto.FromEntity)!;
    }

    public async Task<RestaurantDto?> GetRestaurantByIdAsync(string id)
    {
        logger.LogInformation($"Getting restaurant with id: {id}");
        var restaurant = await repository.GetByIdAsync(id);

        return RestaurantDto.FromEntity(restaurant);
    }
}