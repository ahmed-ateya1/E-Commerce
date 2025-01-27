using E_Commerce.Core.Dtos.CategoryDto;
using FluentValidation;

namespace E_Commerce.Core.Validators.CategoryValidator
{
    public class CategoryBaseValidator : AbstractValidator<CategoryBase>
    {
        public CategoryBaseValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category Name Can't be Blank !")
                .MaximumLength(50).WithMessage("Category Name Can't be More than 50 Characters !");
        }
    }
}
