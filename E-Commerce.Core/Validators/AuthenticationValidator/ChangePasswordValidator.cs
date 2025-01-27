using E_Commerce.Core.Dtos.AuthenticationDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.AuthenticationValidator
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Old password is required")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")
                .Length(8, 100).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")
                .Length(8, 100).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.NewPassword).WithMessage("Password and confirmation password is not match")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")
                .Length(8, 100).WithMessage("Password must be at least 8 characters long.");

        }
    }
}
