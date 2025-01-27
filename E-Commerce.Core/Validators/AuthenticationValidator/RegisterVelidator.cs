using E_Commerce.Core.Dtos.AuthenticationDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.AuthenticationValidator
{
    public class RegisterVelidator : AbstractValidator<RegisterDTO>
    {
        public RegisterVelidator()
        {
            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("Email is required")
                 .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName Name is required")
                .MaximumLength(50).WithMessage("First Name can't be longer than 50 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

            RuleFor(x=>x.Role)
                .IsInEnum().WithMessage("Invalid Role");
        }
    }
}
