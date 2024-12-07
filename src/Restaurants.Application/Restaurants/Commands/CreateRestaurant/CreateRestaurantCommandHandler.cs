using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
    ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository repository,
    IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, Guid>
{
    public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        if (currentUser == null)
        {
            logger.LogWarning("UserId is null");
            throw new BadRequestException("Current user cannot be null");
        }

        logger.LogInformation("{UserEmail} {UserId} is creating new restaurant: {@Restaurant}", currentUser.Email,
            currentUser.Id, request);
        var restaurant = mapper.Map<Restaurant>(request);

        restaurant = await repository.CreateAsync(restaurant);
        restaurant.OwnerId = currentUser.Id;

        var success = await repository.SaveChangesAsync();

        if (success <= 0) throw new Exception("Failed to create new restaurant");

        return restaurant.Id;
    }
}