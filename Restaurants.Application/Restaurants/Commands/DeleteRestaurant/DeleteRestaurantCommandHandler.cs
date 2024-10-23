using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(
    ILogger<DeleteRestaurantCommandHandler> logger,
    IRestaurantsRepository repository) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with id: {RestaurantId}", request.Id);

        var restaurant = await repository.GetByIdAsync(request.Id);
        if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        restaurant.IsActive = false;
        restaurant.Dishes
            .ForEach(d => d.IsActive = false);
        await repository.SaveChangesAsync();
    }
}