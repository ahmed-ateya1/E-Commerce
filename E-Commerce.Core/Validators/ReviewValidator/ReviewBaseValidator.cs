using E_Commerce.Core.Dtos.ReviewDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.ReviewValidator
{
    public class ReviewBaseValidator : AbstractValidator<ReviewBase>
    {
        public ReviewBaseValidator()
        {
            RuleFor(x=>x.ProductID)
                .NotEmpty().WithMessage("Product is required");

            RuleFor(x => x.Rating)
                .NotEmpty().WithMessage("Rating is required")
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.ReviewText)
                .NotEmpty().WithMessage("Review text is required")
                .MaximumLength(500).WithMessage("Review text cannot be more than 500 characters");
        }
    }
}
