using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository repository, ILogger<RestaurantsService> logger, IMapper mapper)
    : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await repository.GetAllAsync();

        return mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }

    public async Task<RestaurantDto?> GetRestaurantByIdAsync(string id)
    {
        logger.LogInformation($"Getting restaurant with id: {id}");
        var restaurant = await repository.GetByIdAsync(id);

        return mapper.Map<RestaurantDto?>(restaurant);
    }
}