using E_Commerce.Core.Dtos.DeliveryMethodDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.DeliveryMethodValidator
{
    public class DeliveryMethodUpdateValidator : AbstractValidator<DeliveryMethodUpdateRequest>
    {
        public DeliveryMethodUpdateValidator()
        {
            RuleFor(x => x.ShortName)
                .NotEmpty().WithMessage("Short Name is required.")
                .Length(1, 50).WithMessage("Short Name can't be longer than 50 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(1, 100).WithMessage("Description can't be longer than 100 characters.");

            RuleFor(x => x.DeliveryTime)
                .NotEmpty().WithMessage("Delivery Time is required.")
                .Length(1, 50).WithMessage("Delivery Time can't be longer than 50 characters.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x=>x.DeliveryMethodID)
                .NotEmpty().WithMessage("Delivery Method ID is required.");

        }
    }
}
