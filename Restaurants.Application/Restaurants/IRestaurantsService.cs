using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantsService
{
    Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync();

    Task<RestaurantDto?> GetRestaurantByIdAsync(string id);
    Task<RestaurantDto> CreateRestaurantAsync(CreateRestaurantDto newRestaurant);
}