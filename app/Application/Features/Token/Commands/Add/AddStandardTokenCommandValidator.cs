using FluentValidation;

namespace Application.Features.Token.Commands.Add
{
    public class AddStandardTokenCommandValidator : AbstractValidator<AddStandardTokenCommand>
    {
        public AddStandardTokenCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.ContractAddress)
               .NotEmpty().WithMessage("Contract Address is required.")
               .NotNull().WithMessage("Contract Address is required.");
        }
    }
}
