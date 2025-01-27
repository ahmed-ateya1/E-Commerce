using E_Commerce.Core.Dtos.ProductDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.ProductValidator
{
    public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateRequestValidator()
        {
            RuleFor(x => x.ProductID)
                .NotEmpty().WithMessage("Product ID can't be blank.");
        }
    }
}
