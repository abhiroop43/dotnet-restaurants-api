using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any()) await dbContext.Database.MigrateAsync();

        if (!await dbContext.Database.CanConnectAsync()) return;


        if (!dbContext.Restaurants.Any())
        {
            var restaurants = GetRestaurants();
            await dbContext.Restaurants.AddRangeAsync(restaurants);
            await dbContext.SaveChangesAsync();
        }

        if (!dbContext.Roles.Any())
        {
            var roles = GetRoles();
            await dbContext.Roles.AddRangeAsync(roles);
            await dbContext.SaveChangesAsync();
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles =
        [
            new(UserRoles.User)
            {
                NormalizedName = UserRoles.User.ToUpper()
            },
            new(UserRoles.Admin)
            {
                NormalizedName = UserRoles.Admin.ToUpper()
            },
            new(UserRoles.Owner)
            {
                NormalizedName = UserRoles.Owner.ToUpper()
            }
        ];

        return roles;
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        var owner = new User
        {
            Email = "seed-user@test.com",
            NormalizedEmail = "seed-user@test.com",
            UserName = "seed-user@test.com",
            NormalizedUserName = "seed-user@test.com",
            EmailConfirmed = true
        };
        return new List<Restaurant>
        {
            new()
            {
                Owner = owner,
                Name = "The Gourmet Kitchen",
                Category = "Italian",
                Description = "Authentic Italian cuisine",
                ContactEmail = "nunzio@gourmetkitchen.com",
                ContactNumber = "123-456-7890",
                HasDelivery = true,
                IsActive = true,
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Metropolis",
                    PostalCode = "10001"
                },
                Dishes =
                [
                    new Dish
                    {
                        Name = "Spaghetti Carbonara", Price = 12.99m,
                        Description = "Pasta with bacon, eggs, and cheese", KiloCalories = 800,
                        IsActive = true
                    },
                    new Dish
                    {
                        Name = "Margherita Pizza", Price = 10.99m,
                        Description = "Pizza with tomato, mozzarella, and basil", KiloCalories = 1200,
                        IsActive = true
                    }
                ]
            },
            new()
            {
                Owner = owner,
                Name = "Sushi World",
                Category = "Japanese",
                Description = "Fresh sushi and sashimi",
                ContactEmail = "banzai@sushiworld.co.jp",
                ContactNumber = "987-654-3210",
                HasDelivery = true,
                IsActive = true,
                Address = new Address
                {
                    Street = "456 Elm St",
                    City = "Gotham",
                    PostalCode = "90001"
                },
                Dishes =
                [
                    new Dish
                    {
                        Name = "California Roll", Price = 8.99m, Description = "Crab, avocado, and cucumber",
                        KiloCalories = 600,
                        IsActive = true
                    },
                    new Dish
                    {
                        Name = "Spicy Tuna Roll", Price = 9.99m, Description = "Tuna, spicy mayo, and cucumber",
                        KiloCalories = 700,
                        IsActive = true
                    }
                ]
            }
        };
    }
}