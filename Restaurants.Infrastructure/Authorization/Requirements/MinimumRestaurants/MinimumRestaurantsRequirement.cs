using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimumRestaurants;

public class MinimumRestaurantsRequirement(int minimumRestaurantsCount) : IAuthorizationRequirement
{
    public int MinimumRestaurantsCount { get; } = minimumRestaurantsCount;
}