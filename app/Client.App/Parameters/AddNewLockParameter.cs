using FluentValidation;
using System;

namespace Client.App.Parameters
{
    public class AddNewLockParameter
    {
        public string Sender { get; set; }
        public string TokenContractAddress { get; set; }
        public string TokenInfo { get; set; }
        public int TokenDecimal { get; set; }
        public long TotalAmount { get; set; }
        public string RecipientAddress { get; set; }
        public DateTime? UnlockDate { get; set; }
        public bool IsRevocable { get; set; }
    }

    public class AddNewLockParameterValidator : AbstractValidator<AddNewLockParameter>
    {
        public AddNewLockParameterValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.TotalAmount)
                 .GreaterThan(0).WithMessage("Amount must be greater than to 0.");

            RuleFor(v => v.RecipientAddress)
                .NotEmpty().WithMessage("Recipient Address is required.")
                .NotNull().WithMessage("Recipient Address is required.");
        }
    }
}
