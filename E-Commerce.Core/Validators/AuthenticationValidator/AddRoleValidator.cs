using E_Commerce.Core.Dtos.AuthenticationDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.AuthenticationValidator
{
    public class AddRoleValidator : AbstractValidator<AddRoleDTO>
    {
        public AddRoleValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Role name is required");
            RuleFor(x => x.UserID).NotEmpty().WithMessage("User ID is required");
        }
    }
}
