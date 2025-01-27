using E_Commerce.Core.Dtos.ProductDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.ProductValidator
{
    public class ProductBaseValidator : AbstractValidator<ProductBase>
    {
        public ProductBaseValidator()
        {
            RuleFor(x=>x.ProductName)
                .NotEmpty().WithMessage("Product Name can't be blank.")
                .MaximumLength(50).WithMessage("Product Name can't be longer than 50 characters.");


            RuleFor(x=>x.ProductDescription)
                .NotEmpty().WithMessage("Product Description can't be blank.")
                .MaximumLength(500).WithMessage("Product Description can't be longer than 500 characters.");

            RuleFor(x => x.ProductPrice)
                .NotEmpty().WithMessage("Product Price can't be blank.")
                .GreaterThan(0).WithMessage("Product Price must be greater than 0.");

            RuleFor(x => x.Discount)
                .NotEmpty().WithMessage("Product Discount can't be blank.")
                .InclusiveBetween(0, 100).WithMessage("Discount must be between 0 and 100.");

            RuleFor(x => x.StockQuantity)
                .NotEmpty().WithMessage("Stock Quantity can't be blank.")
                .GreaterThan(0).WithMessage("Stock Quantity must be greater than 0.");

            RuleFor(x => x.WarrantyPeriod)
                .GreaterThan(0).WithMessage("Warranty Period must be greater than 0.");

            RuleFor(x => x.Color)
                .MaximumLength(50).WithMessage("Color can't be longer than 50 characters.");

            RuleFor(x => x.ModelNumber)
                .NotEmpty().WithMessage("ModelNumber can't be blank.")
                .GreaterThan(0).WithMessage("ModelNumber must be greater than 0.");

            RuleFor(x => x.BrandID)
                .NotEmpty().WithMessage("BrandID can't be blank.");

            RuleFor(x => x.CategoryID)
                .NotEmpty().WithMessage("CategoryID can't be blank.");

        }
    }
}
