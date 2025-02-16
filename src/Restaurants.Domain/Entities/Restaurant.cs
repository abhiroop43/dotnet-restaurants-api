namespace Restaurants.Domain.Entities;

public class Restaurant
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public bool HasDelivery { get; set; }

    public string? ContactEmail { get; set; }
    public string? ContactNumber { get; set; }

    public Address? Address { get; set; }
    public List<Dish> Dishes { get; set; } = [];
    public required bool IsActive { get; set; } = true;
    public User Owner { get; set; } = default!;
    public string OwnerId { get; set; } = default!;
    public string? LogoUrl { get; set; }
}