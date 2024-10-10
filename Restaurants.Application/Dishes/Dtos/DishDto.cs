using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.Dtos;

public class DishDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }

    public static DishDto? FromEntity(Dish? dish)
    {
        if (dish == null) return null;
        return new DishDto
        {
            Description = dish.Description,
            Id = dish.Id,
            KiloCalories = dish.KiloCalories,
            Name = dish.Name,
            Price = dish.Price
        };
    }
}