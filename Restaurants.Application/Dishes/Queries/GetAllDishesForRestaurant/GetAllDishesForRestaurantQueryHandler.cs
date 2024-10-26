using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant;

public class
    GetAllDishesForRestaurantQueryHandler(
        ILogger<GetAllDishesForRestaurantQueryHandler> logger,
        IDishesRepository repository,
        IMapper mapper) : IRequestHandler<GetAllDishesForRestaurantQuery, IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetAllDishesForRestaurantQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all dishes for restaurant id: {@RestaurantId}", request.RestaurantId);
        var dishes = await repository.GetAllAsync(request.RestaurantId);

        return mapper.Map<IEnumerable<DishDto>>(dishes);
    }
}