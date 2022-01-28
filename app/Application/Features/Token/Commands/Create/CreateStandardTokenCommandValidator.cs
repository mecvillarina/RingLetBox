using FluentValidation;

namespace Application.Features.Token.Commands.Create
{
    public class CreateStandardTokenCommandValidator : AbstractValidator<CreateStandardTokenCommand>
    {
        public CreateStandardTokenCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.Symbol)
               .NotEmpty().WithMessage("Symbol is required.")
               .NotNull().WithMessage("Symbol is required.");

            RuleFor(v => v.TotalSupply)
                .GreaterThan(0).WithMessage("Total Supply must be greater than to 0.");

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required.")
                .NotNull().WithMessage("Name is required.");

            RuleFor(v => v.Decimal)
                .GreaterThanOrEqualTo(0).WithMessage("Decimal must be greater than or equal to 0.")
                .LessThanOrEqualTo(8).WithMessage("Decimal must be less than or equal to 8.");
        }
    }
}
