using E_Commerce.Core.Dtos.AuthenticationDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.AuthenticationValidator
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDTO>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");
        }
    }
}
