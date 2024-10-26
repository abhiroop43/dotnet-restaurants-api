using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant;

public class
    GetAllDishesForRestaurantQueryHandler : IRequestHandler<GetAllDishesForRestaurantQuery, IEnumerable<DishDto>>
{
    public Task<IEnumerable<DishDto>> Handle(GetAllDishesForRestaurantQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}