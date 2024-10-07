using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                await dbContext.Restaurants.AddRangeAsync(restaurants);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>
        {
            new()
            {
                Name = "The Gourmet Kitchen",
                Category = "Italian",
                Description = "Authentic Italian cuisine",
                ContactEmail = "nunzio@gourmetkitchen.com",
                ContactNumber = "123-456-7890",
                HasDelivery = true,
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Metropolis",
                    PostalCode = "10001"
                },
                Dishes =
                [
                    new Dish { Name = "Spaghetti Carbonara", Price = 12.99m, Description = "Pasta with bacon, eggs, and cheese" },
                    new Dish { Name = "Margherita Pizza", Price = 10.99m, Description = "Pizza with tomato, mozzarella, and basil" }
                ]
            },
            new()
            {
                Name = "Sushi World",
                Category = "Japanese",
                Description = "Fresh sushi and sashimi",
                ContactEmail = "banzai@sushiworld.co.jp",
                ContactNumber = "987-654-3210",
                HasDelivery = true,
                Address = new Address
                {
                    Street = "456 Elm St",
                    City = "Gotham",
                    PostalCode = "90001"
                },
                Dishes =
                [
                    new Dish { Name = "California Roll", Price = 8.99m, Description = "Crab, avocado, and cucumber" },
                    new Dish { Name = "Spicy Tuna Roll", Price = 9.99m, Description = "Tuna, spicy mayo, and cucumber" }
                ]
            }
        };
    }
}