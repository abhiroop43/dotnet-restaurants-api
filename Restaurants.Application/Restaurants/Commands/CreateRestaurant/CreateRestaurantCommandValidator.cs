using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> _validCategories = new()
    {
        "Fast Food",
        "Traditional",
        "Vegetarian",
        "Vegan",
        "Italian",
        "Mexican",
        "Chinese",
        "Japanese",
        "American",
        "Other"
    };

    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .Length(3, 500);

        RuleFor(dto => dto.Category)
            .NotEmpty()
            .Length(3, 50)
            // .Custom((value, context) =>
            // {
            //     var isValidCategory = validCategories.Contains(value);
            //
            //     if (!isValidCategory) context.AddFailure("Category", "Please provide a valid category");
            // })
            .Must(_validCategories.Contains)
            .WithMessage("Please provide a valid category");


        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address");

        RuleFor(dto => dto.ContactNumber)
            .Matches(@"^(\+\d{1,2}\s?)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")
            .WithMessage("Please provide a valid phone number");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Postal code should be of the format {xx-xxx}");
    }
}