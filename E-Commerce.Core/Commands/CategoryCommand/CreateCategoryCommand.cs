using E_Commerce.Core.Dtos.CategoryDto;
using MediatR;

namespace E_Commerce.Core.Commands.CategoryCommand
{
    public class CreateCategoryCommand : IRequest<CategoryResponse>
    {
        public CategoryAddRequest CategoryAddRequest { get;}

        public CreateCategoryCommand(CategoryAddRequest categoryAddRequest)
        {
            CategoryAddRequest = categoryAddRequest;
        }
    }
}
