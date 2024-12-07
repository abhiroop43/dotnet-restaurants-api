#nullable enable
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Controllers;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.API.Tests.Controllers;

[TestSubject(typeof(RestaurantsController))]
public class RestaurantsControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                    _ => _restaurantsRepositoryMock.Object));
            });
        });
    }

    [Fact]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        // arrange

        var client = _factory.CreateClient();

        // act

        var response = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // arrange

        var client = _factory.CreateClient();

        // act

        var response = await client.GetAsync("/api/restaurants");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        // arrange

        var id = Guid.NewGuid();
        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

        var client = _factory.CreateClient();

        // act

        var response = await client.GetAsync($"/api/restaurants/{id}");

        // assert

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_ForExistingId_ShouldReturn200Ok()
    {
        // arrange

        var id = Guid.NewGuid();
        var restaurant = new Restaurant
        {
            Id = id,
            Name = "Test Restaurant",
            Description = "Test Restaurant",
            HasDelivery = true,
            Category = "Indian",
            IsActive = true
        };
        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);

        var client = _factory.CreateClient();

        // act

        var response = await client.GetAsync($"/api/restaurants/{id}");
        var responseRestaurant = await response.Content.ReadFromJsonAsync<RestaurantDto>();

        // assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseRestaurant.Should().NotBeNull();
        responseRestaurant.Name.Should().Be(restaurant.Name);
        responseRestaurant.Description.Should().Be(restaurant.Description);
    }
}