using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

[TestSubject(typeof(CreateRestaurantCommandHandler))]
public class CreateRestaurantCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {
        // arrange
        var restaurantId = Guid.NewGuid();
        var currentUserId = Guid.NewGuid().ToString();
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();

        var mapperMock = new Mock<IMapper>();
        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Traditional",
            IsActive = true
        };
        mapperMock.Setup(m => m.Map<CreateRestaurantCommand, Restaurant>(command)).Returns(restaurant);

        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Restaurant>()))
            .ReturnsAsync(restaurant);
        restaurantRepositoryMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser(
            currentUserId, "test@test.com", [UserRoles.Owner], "IND", null
        );
        userContextMock.Setup(usr => usr.GetCurrentUser()).Returns(currentUser);

        var commandHandler = new CreateRestaurantCommandHandler(loggerMock.Object, mapperMock.Object,
            restaurantRepositoryMock.Object, userContextMock.Object);

        // act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // assert
        result.Should().Be(restaurantId);
        restaurant.OwnerId.Should().Be(currentUserId);
        restaurantRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Restaurant>()), Times.Once);
        restaurantRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}