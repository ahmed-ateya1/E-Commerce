using E_Commerce.Core.Dtos.ProductDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.ProductValidator
{
    public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
    {
        public ProductAddRequestValidator()
        {
            RuleFor(x => x.ProductFiles)
                .NotEmpty().WithMessage("must select at least one image");
        }
    }
}
