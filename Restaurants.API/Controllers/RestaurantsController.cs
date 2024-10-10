using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Exceptions;
using Restaurants.Application.Restaurants;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await restaurantsService.GetAllRestaurantsAsync();
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var restaurant = await restaurantsService.GetRestaurantByIdAsync(id);
            return Ok(restaurant);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}