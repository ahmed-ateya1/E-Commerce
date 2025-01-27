using E_Commerce.Core.Dtos.WishlistDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.WishlistValidator
{
    public class WishlistUpdateRequestValidator : AbstractValidator<WishlistUpdateRequest>
    {
        public WishlistUpdateRequestValidator()
        {
            RuleFor(x=>x.WishlistID)
                .NotEmpty().WithMessage("Wishlist can't be blank.");
        }
    }
}
