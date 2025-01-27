using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.SpecificationValidator
{
    public class TechnicalSpecificationValidator : AbstractValidator<TechnicalSpecificationBase>
    {
        public TechnicalSpecificationValidator()
        {
            RuleFor(x=>x.ProductID)
                .NotEmpty().WithMessage("Product Id can't be blank.");
            RuleFor(x => x.SpecificationKey)
                .NotEmpty().WithMessage("Specification Key can't be blank.")
                .MaximumLength(100).WithMessage("Specification Key can't be more than 100 characters.");

            RuleFor(x => x.SpecificationValue)
                .NotEmpty().WithMessage("Specification Value can't be blank.")
                .MaximumLength(500).WithMessage("Specification Value can't be more than 500 characters.");

        }
    }
}
