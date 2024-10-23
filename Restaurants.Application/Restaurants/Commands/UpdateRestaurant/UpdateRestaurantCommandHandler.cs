using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger<UpdateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository repository) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant: {@UpdatedRestaurant}", request);
        var restaurant = await repository.GetByIdAsync(request.Id);

        if (restaurant == null) throw new NotFoundException($"No restaurant found with id: {request.Id}");

        mapper.Map(request, restaurant);
        await repository.SaveChangesAsync();
    }
}