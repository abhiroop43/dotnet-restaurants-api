using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Persistence;

internal class RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options, IConfiguration configuration)
    : IdentityDbContext<User>(options)
{
  internal DbSet<Restaurant> Restaurants { get; set; }
  internal DbSet<Dish> Dishes { get; set; }
  internal DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.HasDefaultSchema(configuration.GetSection("dbSchema").Value);
    modelBuilder.Entity<Restaurant>().OwnsOne(r => r.Address);

    modelBuilder.Entity<Restaurant>()
        .HasMany(r => r.Dishes)
        .WithOne()
        .HasForeignKey(d => d.RestaurantId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<User>()
        .HasMany(o => o.OwnedRestaurants)
        .WithOne(r => r.Owner)
        .HasForeignKey(r => r.OwnerId);

    modelBuilder.Entity<User>()
        .HasOne(t => t.UserRefreshToken)
        .WithOne()
        .HasForeignKey<UserRefreshToken>(t => t.UserId);
  }
}
