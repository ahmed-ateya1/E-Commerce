using E_Commerce.Core.Dtos.BrandDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.BrandValidator
{
    public class BrandBaseValidator : AbstractValidator<BrandBase>
    {
        public BrandBaseValidator()
        {
            RuleFor(x=>x.BrandName)
                .NotEmpty().WithMessage("Brand Name can't be blank .")
                .Length(1, 50).WithMessage("Brand Name can't be longer than 50 characters.");


        }
    }
}
