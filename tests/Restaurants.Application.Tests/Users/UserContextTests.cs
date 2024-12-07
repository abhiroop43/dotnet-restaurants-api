using System.Security.Claims;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Tests.Users;

[TestSubject(typeof(UserContext))]
public class UserContextTests
{
    [Fact]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var dateOfBirth = new DateOnly(1962, 11, 18);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("Nationality", "IND"),
            new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
        {
            User = user
        });
        var userContext = new UserContext(httpContextAccessorMock.Object);

        var currentUser = userContext.GetCurrentUser();

        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().Contain(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("IND");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

        var userContext = new UserContext(httpContextAccessorMock.Object);

        Action act = () => userContext.GetCurrentUser();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User context is not present");
    }
}