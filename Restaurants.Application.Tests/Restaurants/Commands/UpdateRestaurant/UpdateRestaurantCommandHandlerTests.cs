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
    private readonly UpdateRestaurantCommandHandler _handler;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;

    public UpdateRestaurantCommandHandlerTests()
    {
        Mock<ILogger<UpdateRestaurantCommandHandler>> loggerMock = new();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

        _handler = new UpdateRestaurantCommandHandler(
            loggerMock.Object,
            _mapperMock.Object,
            _restaurantsRepositoryMock.Object,
            _restaurantAuthorizationServiceMock.Object);
    }

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

        _mapperMock.Setup(mapper => mapper.Map<UpdateRestaurantCommand, Restaurant>(command)).Returns(restaurant);

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(restaurant);
        _restaurantsRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        _restaurantAuthorizationServiceMock.Setup(r => r.Authorize(It.IsAny<Restaurant>(), ResourceOperation.Update))
            .Returns(true);

        await _handler.Handle(command, CancellationToken.None);

        _restaurantsRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _restaurantsRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _restaurantAuthorizationServiceMock.Verify(r => r.Authorize(It.IsAny<Restaurant>(), ResourceOperation.Update),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ForInvalidCommand_ShouldThrowNotFoundException()
    {
        var restaurantId = Guid.NewGuid();

        var command = new UpdateRestaurantCommand
        {
            Id = Guid.NewGuid(),
            Description = "Test updated Description",
            HasDelivery = false,
            Name = "Test Updated Name"
        };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync((Restaurant?)null);


        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _restaurantsRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _restaurantsRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        _restaurantAuthorizationServiceMock.Verify(r => r.Authorize(It.IsAny<Restaurant>(), ResourceOperation.Update),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithUnauthorizedUser_ShouldThrowForbiddenException()
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


        _mapperMock.Setup(mapper => mapper.Map<UpdateRestaurantCommand, Restaurant>(command)).Returns(restaurant);

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock
            .Setup(a => a.Authorize(restaurant, ResourceOperation.Update))
            .Returns(false);


        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>();
        _restaurantsRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _restaurantAuthorizationServiceMock.Verify(r => r.Authorize(restaurant, ResourceOperation.Update),
            Times.Once);
        _restaurantsRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}