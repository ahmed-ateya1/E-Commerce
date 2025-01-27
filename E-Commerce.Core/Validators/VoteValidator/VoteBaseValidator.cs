using E_Commerce.Core.Dtos.VoteDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.VoteValidator
{
    public class VoteBaseValidator : AbstractValidator<VoteBase>
    {
        public VoteBaseValidator()
        {
            RuleFor(x=>x.ReviewID)
                .NotEmpty().WithMessage("Review is required");

        }
    }
}
