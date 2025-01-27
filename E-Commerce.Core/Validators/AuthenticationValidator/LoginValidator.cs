using E_Commerce.Core.Dtos.AuthenticationDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.AuthenticationValidator
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid")
                .MaximumLength(100).WithMessage("Email is too long");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
