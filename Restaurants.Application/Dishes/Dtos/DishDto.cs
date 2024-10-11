namespace Restaurants.Application.Dishes.Dtos;

public class DishDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }
}