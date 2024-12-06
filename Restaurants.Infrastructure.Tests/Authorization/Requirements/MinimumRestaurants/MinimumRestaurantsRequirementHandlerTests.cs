using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements.MinimumRestaurants;

namespace Restaurants.Infrastructure.Tests.Authorization.Requirements.MinimumRestaurants;

[TestSubject(typeof(MinimumRestaurantsRequirementHandler))]
public class MinimumRestaurantsRequirementHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // arrange
        var currentUser = new CurrentUser(Guid.NewGuid().ToString(), "test@test.com", [], "GER", null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Restaurant 1",
                Description = "Test Restaurant 1",
                Category = "Traditional",
                IsActive = true,
                OwnerId = currentUser.Id
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Restaurant 2",
                Description = "Test Restaurant 2",
                Category = "Chinese",
                IsActive = true,
                OwnerId = currentUser.Id
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Restaurant 3",
                Description = "Test Restaurant 3",
                Category = "Italian",
                IsActive = true,
                OwnerId = currentUser.Id
            }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllByOwnerAsync(currentUser.Id)).ReturnsAsync(restaurants);

        Mock<ILogger<MinimumRestaurantsRequirementHandler>> loggerMock = new();

        var requirement = new MinimumRestaurantsRequirement(2);
        var handler = new MinimumRestaurantsRequirementHandler(loggerMock.Object, userContextMock.Object,
            restaurantsRepositoryMock.Object);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // act
        await handler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
    {
        // arrange
        var currentUser = new CurrentUser(Guid.NewGuid().ToString(), "test@test.com", [], "GER", null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Restaurant 1",
                Description = "Test Restaurant 1",
                Category = "Traditional",
                IsActive = true,
                OwnerId = currentUser.Id
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Restaurant 2",
                Description = "Test Restaurant 2",
                Category = "Chinese",
                IsActive = true,
                OwnerId = currentUser.Id
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Restaurant 3",
                Description = "Test Restaurant 3",
                Category = "Italian",
                IsActive = true,
                OwnerId = currentUser.Id
            }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllByOwnerAsync(currentUser.Id)).ReturnsAsync(restaurants);

        Mock<ILogger<MinimumRestaurantsRequirementHandler>> loggerMock = new();

        var requirement = new MinimumRestaurantsRequirement(5);
        var handler = new MinimumRestaurantsRequirementHandler(loggerMock.Object, userContextMock.Object,
            restaurantsRepositoryMock.Object);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // act
        await handler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
    }
}