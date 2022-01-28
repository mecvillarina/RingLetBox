using FluentValidation;

namespace Application.Features.SmartContract.Commands.Create
{
    public class CreateSmartContractCommandValidator : AbstractValidator<CreateSmartContractCommand>
    {
        public CreateSmartContractCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.Sender)
               .NotEmpty().WithMessage("Sender is required.")
               .NotNull().WithMessage("Sender is required.");


            RuleFor(v => v.ContractCode)
               .NotEmpty().WithMessage("Sender is required.")
               .NotNull().WithMessage("Sender is required.");
        }
    }
}
