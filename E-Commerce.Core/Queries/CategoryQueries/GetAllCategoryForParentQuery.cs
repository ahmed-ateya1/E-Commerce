using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.CategoryDto;
using MediatR;

namespace E_Commerce.Core.Queries.CategoryQueries
{
    public class GetAllCategoryForParentQuery : IRequest<IEnumerable<CategoryResponse>>
    {
        public Guid ParentCategoryID { get; set; }

        public GetAllCategoryForParentQuery(Guid parentCategoryID)
        {
            ParentCategoryID = parentCategoryID;
        }
    }
}
