using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Tests.Restaurants.Dtos;

[TestSubject(typeof(RestaurantsProfile))]
public class RestaurantsProfileTests
{
    private readonly IMapper _mapper;

    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<RestaurantsProfile>(); });

        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // arrange
        var restaurant = new Restaurant
        {
            Id = Guid.NewGuid(),
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Fast Food",
            HasDelivery = true,
            ContactEmail = "test@test.com",
            ContactNumber = "123456789",
            Address = new Address
            {
                Street = "Test Street",
                City = "Test City",
                PostalCode = "12345"
            },
            IsActive = true
        };

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        // assert
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
    }

    [Fact]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange
        var createRestaurantCommand = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Fast Food",
            HasDelivery = true,
            ContactEmail = "test@test.com",
            ContactNumber = "123456789",
            PostalCode = "12-345",
            City = "Test City",
            Street = "Test Street"
        };

        var restaurant = _mapper.Map<Restaurant>(createRestaurantCommand);

        // assert
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(createRestaurantCommand.Name);
        restaurant.Description.Should().Be(createRestaurantCommand.Description);
        restaurant.Category.Should().Be(createRestaurantCommand.Category);
        restaurant.HasDelivery.Should().Be(createRestaurantCommand.HasDelivery);
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.City.Should().Be(createRestaurantCommand.City);
        restaurant.Address.PostalCode.Should().Be(createRestaurantCommand.PostalCode);
        restaurant.Address.Street.Should().Be(createRestaurantCommand.Street);
    }

    [Fact]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange
        var updateRestaurantCommand = new UpdateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            HasDelivery = true,
            Id = Guid.NewGuid()
        };

        var restaurant = _mapper.Map<Restaurant>(updateRestaurantCommand);

        // assert
        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(updateRestaurantCommand.Id);
        restaurant.Name.Should().Be(updateRestaurantCommand.Name);
        restaurant.Description.Should().Be(updateRestaurantCommand.Description);
        restaurant.HasDelivery.Should().Be(updateRestaurantCommand.HasDelivery);
    }
}