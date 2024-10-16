using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

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
        var restaurant = await restaurantsService.GetRestaurantByIdAsync(id);

        if (restaurant == null) return NotFound("This restaurant does not exist");

        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantDto newRestaurant)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var restaurant = await restaurantsService.CreateRestaurantAsync(newRestaurant);
            return CreatedAtAction(nameof(GetById), new { id = restaurant.Id }, restaurant);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}