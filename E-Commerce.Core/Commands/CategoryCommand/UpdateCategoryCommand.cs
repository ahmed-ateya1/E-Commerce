using E_Commerce.Core.Dtos.CategoryDto;
using MediatR;

namespace E_Commerce.Core.Commands.CategoryCommand
{
    public class UpdateCategoryCommand : IRequest<CategoryResponse>
    {
        public CategoryUpdateRequest CategoryUpdateRequest { get; }

        public UpdateCategoryCommand(CategoryUpdateRequest categoryUpdateRequest)
        {
            CategoryUpdateRequest = categoryUpdateRequest;
        }
    }
}
