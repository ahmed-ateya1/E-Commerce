using FluentValidation;

namespace E_Commerce.Core.Validators.DealValidator
{
    public class DealAddValidator : AbstractValidator<DealAddRequest>
    {
        public DealAddValidator()
        {
            RuleFor(x=>x.ProductID)
                .NotEmpty().WithMessage("Product can't be blank!");

            RuleFor(x=>x.Discount)
                .NotEmpty().WithMessage("Discount can't be blank!")
                .InclusiveBetween(0,100).WithMessage("Discount must be between 0 and 100!");

            RuleFor(x=>x.StartDate)
                .NotEmpty().WithMessage("Start date can't be blank!")
                .LessThan(x => x.EndDate).WithMessage("Start date must be less than end date!");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date can't be blank!")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be greater than start date!");
        }
    }
}
