using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/dishes")]
[ApiController]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllDishesForRestaurant([FromRoute] Guid restaurantId)
    {
        var dishes = await mediator.Send(new GetAllDishesForRestaurantQuery(restaurantId));
        return Ok(dishes);
    }

    [HttpGet("{dishId}")]
    public async Task<IActionResult> GetDishById([FromRoute] Guid restaurantId, [FromRoute] Guid dishId)
    {
        return Ok();
    }

    [HttpPost("")]
    public async Task<IActionResult> AddNewDish([FromRoute] Guid restaurantId)
    {
        return Created();
    }

    [HttpPatch("")]
    public async Task<IActionResult> UpdateDish([FromRoute] Guid restaurantId)
    {
        return NoContent();
    }

    [HttpDelete("{dishId}")]
    public async Task<IActionResult> DeleteDish([FromRoute] Guid restaurantId, [FromRoute] Guid dishId)
    {
        return NoContent();
    }
}