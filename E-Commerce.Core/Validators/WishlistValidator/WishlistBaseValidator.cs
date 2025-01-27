using E_Commerce.Core.Dtos.WishlistDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.WishlistValidator
{
    public class WishlistBaseValidator : AbstractValidator<WishlistBase>
    {
        public WishlistBaseValidator()
        {
            RuleFor(x=>x.ProductID).NotEmpty().WithMessage("Product can't be blank.");
        }
    }
}
