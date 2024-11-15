using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDish;

public class DeleteDishCommandHandler(
    ILogger<DeleteDishCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteDishCommand>
{
    public async Task Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting dish id {@dishId} for restaurant id {@restaurantId}", request.Id,
            request.RestaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);

        if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbiddenException("This user is not allowed to update the restaurant");

        var dish = await dishesRepository.GetByIdAsync(request.RestaurantId, request.Id);

        if (dish == null) throw new NotFoundException(nameof(Dish), request.Id.ToString());

        dish.IsActive = false;

        await dishesRepository.SaveChangesAsync();
    }
}