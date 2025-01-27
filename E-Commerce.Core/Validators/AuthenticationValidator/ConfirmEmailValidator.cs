using E_Commerce.Core.Dtos.AuthenticationDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.AuthenticationValidator
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailDTO>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email address")
                .NotEmpty().WithMessage("Email is required");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required");
        }
    }
}
