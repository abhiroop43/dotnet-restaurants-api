using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetResturantByIdQueryHandler(IRestaurantsRepository repository, ILogger<GetResturantByIdQueryHandler> logger, IMapper mapper) : IRequestHandler<GetResturantByIdQuery, RestaurantDto?>
{
  public async Task<RestaurantDto?> Handle(GetResturantByIdQuery request, CancellationToken cancellationToken)
  {
    logger.LogInformation($"Getting restaurant with id: {request.Id}");
    var restaurant = await repository.GetByIdAsync(request.Id);

    return mapper.Map<RestaurantDto?>(restaurant);
  }
}