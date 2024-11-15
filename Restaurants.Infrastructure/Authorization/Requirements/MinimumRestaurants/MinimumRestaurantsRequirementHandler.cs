using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimumRestaurants;

public class MinimumRestaurantsRequirementHandler(
    ILogger<MinimumRestaurantsRequirementHandler> logger,
    IUserContext userContext,
    IRestaurantsRepository restaurantsRepository) : AuthorizationHandler<MinimumRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        MinimumRestaurantsRequirement requirement)
    {
        var user = userContext.GetCurrentUser();

        if (user == null)
        {
            logger.LogWarning("user is null");
            context.Fail();
            return;
        }

        logger.LogInformation("User: {Email} handling Minimum Restaurants Authorization", user.Email);

        var restaurantsByUser = await restaurantsRepository.GetAllByOwnerAsync(user.Id);

        if (restaurantsByUser.Count() >= requirement.MinimumRestaurantsCount)
        {
            logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}