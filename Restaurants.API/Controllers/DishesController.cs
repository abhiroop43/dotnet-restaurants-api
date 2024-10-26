using Microsoft.AspNetCore.Mvc;

namespace Restaurants.API.Controllers;

[Microsoft.AspNetCore.Components.Route("api/restaurant/{restaurantId}/dishes")]
[ApiController]
public class DishesController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllDishesForRestaurant([FromRoute] Guid restaurantId)
    {
        return Ok();
    }

    [HttpGet("{dishId}")]
    public async Task<IActionResult> GetDishById([FromRoute] Guid restaurantId, [FromRoute] Guid dishId)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> AddNewDish([FromRoute] Guid restaurantId)
    {
        return Created();
    }

    [HttpPatch]
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