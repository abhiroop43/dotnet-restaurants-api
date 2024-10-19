using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetResturantByIdQuery(Guid id) : IRequest<RestaurantDto?>
{
  public Guid Id { get; } = id;
}