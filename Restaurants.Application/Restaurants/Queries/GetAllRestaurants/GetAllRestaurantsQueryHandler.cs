using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger, IMapper mapper, IRestaurantsRepository repository) : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
{
  public async Task<IEnumerable<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
  {
    logger.LogInformation("Getting all restaurants");
    var restaurants = await repository.GetAllAsync();

    return mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
  }
}