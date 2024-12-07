using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ProductDto;
using MediatR;

namespace E_Commerce.Core.Queries.ProductQueries
{
    public class GetAllProductsQuery : IRequest<PaginatedResponse<ProductResponse>>
    {
        public PaginationDto Pagination { get; set; }

        public GetAllProductsQuery(PaginationDto pagination)
        {
            Pagination = pagination;
        }
    }
}
