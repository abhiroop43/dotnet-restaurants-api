using FluentAssertions;
using JetBrains.Annotations;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Tests.Users;

[TestSubject(typeof(CurrentUser))]
public class CurrentUserTests
{
    [Theory]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        var currentUser = new CurrentUser(Guid.NewGuid().ToString(), "test@test.com", [UserRoles.Admin, UserRoles.User],
            "IND", null);

        var isInRole = currentUser.IsInRole(roleName);

        isInRole.Should().BeTrue();
    }

    [Fact]
    public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        var currentUser = new CurrentUser(Guid.NewGuid().ToString(), "test@test.com", [UserRoles.Admin, UserRoles.User],
            "India", null);

        var isInRole = currentUser.IsInRole(UserRoles.Owner);

        isInRole.Should().BeFalse();
    }

    [Fact]
    public void IsInRole_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        var currentUser = new CurrentUser(Guid.NewGuid().ToString(), "test@test.com", [UserRoles.Admin, UserRoles.User],
            "India", null);

        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

        isInRole.Should().BeFalse();
    }
}