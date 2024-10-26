using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant;

public class GetAllDishesForRestaurantQuery : IRequest<IEnumerable<DishDto>>
{
}