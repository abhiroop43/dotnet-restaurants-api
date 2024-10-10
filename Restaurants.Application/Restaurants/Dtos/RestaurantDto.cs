using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool HasDelivery { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public List<DishDto?> Dishes { get; set; } = [];

    public static RestaurantDto? FromEntity(Restaurant? restaurant)
    {
        if (restaurant == null) return null;
        return new RestaurantDto
        {
            Category = restaurant.Category,
            City = restaurant.Address?.City,
            Description = restaurant.Description,
            Id = restaurant.Id,
            Name = restaurant.Name,
            PostalCode = restaurant.Address?.PostalCode,
            HasDelivery = restaurant.HasDelivery,
            Street = restaurant.Address?.Street,
            Dishes = restaurant.Dishes.Select(DishDto.FromEntity).ToList()
        };
    }
}