using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
    ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository repository) : IRequestHandler<CreateRestaurantCommand, Guid>
{
    public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new restaurant: {@Restaurant}", request);
        var restaurant = mapper.Map<Restaurant>(request);

        restaurant = await repository.CreateAsync(restaurant);
        var success = await repository.SaveChangesAsync();

        if (success <= 0) throw new Exception("Failed to create new restaurant");
        
        return restaurant.Id;
    }
}