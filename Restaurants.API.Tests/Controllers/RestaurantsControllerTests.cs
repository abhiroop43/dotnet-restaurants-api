using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Testing;
using Restaurants.API.Controllers;
using Xunit;

namespace Restaurants.API.Tests.Controllers;

[TestSubject(typeof(RestaurantsController))]
public class RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        // arrange

        var client = factory.CreateClient();

        // act

        var response = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // arrange

        var client = factory.CreateClient();

        // act

        var response = await client.GetAsync("/api/restaurants");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}