using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDish;

public class DeleteDishCommand(Guid dishId, Guid restaurantId) : IRequest
{
    public Guid Id { get; } = dishId;
    public Guid RestaurantId { get; set; } = restaurantId;
}