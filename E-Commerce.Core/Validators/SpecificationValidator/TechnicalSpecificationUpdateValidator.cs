using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.SpecificationValidator
{
    public class TechnicalSpecificationUpdateValidator : AbstractValidator<TechnicalSpecificationUpdateRequest>
    {
        public TechnicalSpecificationUpdateValidator()
        {
           RuleFor(x => x.TechnicalSpecificationID)
                .NotEmpty().WithMessage("Technical Specification can't be blank.");

        }
    }
}
