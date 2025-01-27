using E_Commerce.Core.Dtos.OrderItemsDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.OrderItemValidator
{
    public class OrderItemAddValidator : AbstractValidator<OrderItemAddRequest>
    {
        public OrderItemAddValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0")
                .NotEmpty().WithMessage("Price is required");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .NotEmpty().WithMessage("Quantity is required");

            RuleFor(x => x.ProductID)
                .NotEmpty().WithMessage("ProductID is required");


        }
    }
}
