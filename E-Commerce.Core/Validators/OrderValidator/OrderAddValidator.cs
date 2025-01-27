using E_Commerce.Core.Dtos.OrderDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.OrderValidator
{
    public class OrderAddValidator : AbstractValidator<OrderAddRequest>
    {
        public OrderAddValidator()
        {
            RuleFor(x=>x.AddressID)
                .NotEmpty().WithMessage("AddressID is required");

            RuleFor(x => x.DeliveryMethodID)
                .NotEmpty().WithMessage("DeliveryMethodID is required");

            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("OrderItems is required");
        }
    }
}
