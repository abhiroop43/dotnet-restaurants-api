using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler(
    ILogger<CreateDishCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IMapper mapper) : IRequestHandler<CreateDishCommand, Guid>
{
    public async Task<Guid> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new Dish {@Dish}", request);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);

        if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dishExists = await dishesRepository.CheckIfDishExistsAndIsActive(request.RestaurantId, request.Name);

        if (dishExists)
            throw new BadRequestException(
                $"The dish \"{request.Name}\" already exists for RestaurantId {request.RestaurantId}");

        var dish = mapper.Map<Dish>(request);
        dish = await dishesRepository.CreateAsync(dish);
        var success = await dishesRepository.SaveChangesAsync();

        if (success <= 0) throw new Exception("Failed to create new restaurant");

        return dish.Id;
    }
}