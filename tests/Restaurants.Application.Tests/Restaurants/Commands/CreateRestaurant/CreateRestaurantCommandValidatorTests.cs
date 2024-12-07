using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

[TestSubject(typeof(CreateRestaurantCommandValidator))]
public class CreateRestaurantCommandValidatorTests
{
    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test",
            Description = "Test Restaurant for our Unit Test",
            Category = "Italian",
            ContactEmail = "test@test.com",
            PostalCode = "12-345"
        };

        var validator = new CreateRestaurantCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ForVInvalidCommand_ShouldHaveValidationErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Ha",
            Description = "te",
            Category = "Demo",
            ContactEmail = "123",
            PostalCode = "8888"
        };

        var validator = new CreateRestaurantCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Description);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    [Theory]
    [InlineData("Fast Food")]
    [InlineData("Traditional")]
    [InlineData("Vegetarian")]
    [InlineData("Vegan")]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Chinese")]
    [InlineData("Japanese")]
    [InlineData("American")]
    [InlineData("Other")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategory(string category)
    {
        // arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { Category = category };

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }

    [Theory]
    [InlineData("10220")]
    [InlineData("102-20")]
    [InlineData("10 220")]
    [InlineData("10-2 20")]
    public void Validator_ForInvalidCategory_ShouldHaveValidationErrorsForPostalCode(string postalCode)
    {
        // arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { PostalCode = postalCode };

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }
}