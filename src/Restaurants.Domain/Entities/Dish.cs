namespace Restaurants.Domain.Entities;

public class Dish
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }

    public Guid RestaurantId { get; set; }
    public required bool IsActive { get; set; } = true;
}