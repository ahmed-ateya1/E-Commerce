using E_Commerce.Core.Dtos.CategoryDto;
using MediatR;

namespace E_Commerce.Core.Queries.CategoryQueries
{
    public class GetAllParentCategoryQuery : IRequest<IEnumerable<CategoryResponse>>
    {
    }
}
