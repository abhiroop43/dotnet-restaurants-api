using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishById;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId:guid}/dishes")]
[ApiController]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllDishesForRestaurant([FromRoute] Guid restaurantId)
    {
        var dishes = await mediator.Send(new GetAllDishesForRestaurantQuery(restaurantId));
        return Ok(dishes);
    }

    [HttpGet("{dishId:guid}")]
    public async Task<IActionResult> GetDishById([FromRoute] Guid restaurantId, [FromRoute] Guid dishId)
    {
        var dish = await mediator.Send(new GetDishByIdQuery(restaurantId, dishId));
        return Ok(dish);
    }

    [HttpPost("")]
    public async Task<IActionResult> AddNewDish([FromRoute] Guid restaurantId, [FromBody] CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        var dishId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetDishById), new { restaurantId, dishId }, null);
    }

    [HttpPatch("")]
    public async Task<IActionResult> UpdateDish([FromRoute] Guid restaurantId)
    {
        return NoContent();
    }

    [HttpDelete("{dishId:guid}")]
    public async Task<IActionResult> DeleteDish([FromRoute] Guid restaurantId, [FromRoute] Guid dishId)
    {
        return NoContent();
    }
}