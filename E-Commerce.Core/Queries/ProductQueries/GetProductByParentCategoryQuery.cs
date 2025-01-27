using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ProductDto;
using MediatR;

namespace E_Commerce.Core.Queries.ProductQueries
{
    public class GetProductByParentCategoryQuery : IRequest<PaginatedResponse<ProductResponse>>
    {
        public GetProductByParentCategoryQuery(Guid parentCategoryID, PaginationDto pagination)
        {
            ParentCategoryID = parentCategoryID;
            Pagination = pagination;
        }

        public Guid ParentCategoryID { get; }
        public PaginationDto Pagination { get;}


    }
}
