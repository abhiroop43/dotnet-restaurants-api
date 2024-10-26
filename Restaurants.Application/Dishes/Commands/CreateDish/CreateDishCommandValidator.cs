using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .Length(3, 500);

        RuleFor(dto => dto.Price)
            .NotEmpty()
            .WithMessage("Price of the dish is required")
            .GreaterThan(0.00m)
            .WithMessage("Price must be greater than 0");

        RuleFor(dto => dto.KiloCalories)
            .GreaterThan(0)
            .WithMessage("Kilocalories must be greater than 0");
    }
}