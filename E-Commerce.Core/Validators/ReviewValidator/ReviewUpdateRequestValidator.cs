using E_Commerce.Core.Dtos.ReviewDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.ReviewValidator
{
    public class ReviewUpdateRequestValidator : AbstractValidator<ReviewUpdateRequest>
    {
        public ReviewUpdateRequestValidator()
        {
            RuleFor(x=>x.ReviewID)
                .NotEmpty().WithMessage("Review ID is required");
        }
    }
}
