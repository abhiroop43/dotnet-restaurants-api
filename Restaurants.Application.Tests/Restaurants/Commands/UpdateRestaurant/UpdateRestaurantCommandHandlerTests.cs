using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

[TestSubject(typeof(UpdateRestaurantCommandHandler))]
public class UpdateRestaurantCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForValidCommand_RestaurantUpdated()
    {
        var restaurantId = Guid.NewGuid();

        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Traditional",
            IsActive = true
        };
        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId,
            Description = "Test updated Description",
            HasDelivery = false,
            Name = "Test Updated Name"
        };

        var loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(mapper => mapper.Map<UpdateRestaurantCommand, Restaurant>(command)).Returns(restaurant);

        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(restaurant);
        restaurantRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        var restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        restaurantAuthorizationServiceMock.Setup(r => r.Authorize(It.IsAny<Restaurant>(), ResourceOperation.Update))
            .Returns(true);

        var commandHandler = new UpdateRestaurantCommandHandler(loggerMock.Object, mapperMock.Object,
            restaurantRepositoryMock.Object, restaurantAuthorizationServiceMock.Object);

        await commandHandler.Handle(command, CancellationToken.None);

        restaurantRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        restaurantRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        restaurantAuthorizationServiceMock.Verify(r => r.Authorize(It.IsAny<Restaurant>(), ResourceOperation.Update),
            Times.Once);
    }

    [Fact]
    public void Handle_ForInvalidCommand_ThrowsNotFoundException()
    {
        var restaurantId = Guid.NewGuid();

        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Traditional",
            IsActive = true
        };
        var command = new UpdateRestaurantCommand
        {
            Id = Guid.NewGuid(),
            Description = "Test updated Description",
            HasDelivery = false,
            Name = "Test Updated Name"
        };

        var loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(mapper => mapper.Map<UpdateRestaurantCommand, Restaurant>(command)).Returns(restaurant);

        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);
        // restaurantRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        var restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        // restaurantAuthorizationServiceMock.Setup(r => r.Authorize(It.IsAny<Restaurant>(), ResourceOperation.Update))
        // .Returns(true);

        var commandHandler = new UpdateRestaurantCommandHandler(loggerMock.Object, mapperMock.Object,
            restaurantRepositoryMock.Object, restaurantAuthorizationServiceMock.Object);

        Action act = void () => commandHandler.Handle(command, CancellationToken.None).GetAwaiter().GetResult();

        act.Should().Throw<NotFoundException>();
        restaurantRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        restaurantRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        restaurantAuthorizationServiceMock.Verify(r => r.Authorize(It.IsAny<Restaurant>(), ResourceOperation.Update),
            Times.Never);
    }
    
    [Fact]
    public void Handle_ForInvalidCommand_ThrowsForbiddenException()
    {
        var restaurantId = Guid.NewGuid();

        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Traditional",
            IsActive = true
        };
        var command = new UpdateRestaurantCommand
        {
            Id = Guid.NewGuid(),
            Description = "Test updated Description",
            HasDelivery = false,
            Name = "Test Updated Name"
        };

        var loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(mapper => mapper.Map<UpdateRestaurantCommand, Restaurant>(command)).Returns(restaurant);

        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(restaurant);
        // restaurantRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        var restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        restaurantAuthorizationServiceMock.Setup(r => r.Authorize(restaurant, ResourceOperation.Update))
        .Throws(new ForbiddenException("Test exception"));

        var commandHandler = new UpdateRestaurantCommandHandler(loggerMock.Object, mapperMock.Object,
            restaurantRepositoryMock.Object, restaurantAuthorizationServiceMock.Object);

        Action act = void () => commandHandler.Handle(command, CancellationToken.None).GetAwaiter().GetResult();

        act.Should().Throw<ForbiddenException>();
        restaurantRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        restaurantAuthorizationServiceMock.Verify(r => r.Authorize(restaurant, ResourceOperation.Update),
            Times.Once);
        restaurantRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}