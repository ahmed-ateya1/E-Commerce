using E_Commerce.Core.Dtos.ProductDto;
using E_Commerce.Core.Dtos;
using MediatR;

namespace E_Commerce.Core.Queries.ProductQueries
{
    public class GetTopProductsQuery : IRequest<PaginatedResponse<ProductResponse>>
    {
        public PaginationDto Pagination { get; }

        public GetTopProductsQuery(PaginationDto pagination)
        {
            Pagination = pagination;
        }
    }
}
