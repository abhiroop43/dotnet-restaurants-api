using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger, IMapper mapper, IRestaurantsRepository repository) : IRequestHandler<CreateRestaurantCommand, Guid>
{
  public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
  {
    logger.LogInformation($"Creating new restaurant: {JsonConvert.SerializeObject(request)}");
    var restaurant = mapper.Map<Restaurant>(request);

    restaurant = await repository.CreateAsync(restaurant);
    return restaurant.Id;
  }
}