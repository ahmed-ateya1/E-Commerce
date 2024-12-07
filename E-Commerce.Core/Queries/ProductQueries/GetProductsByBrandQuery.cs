using E_Commerce.Core.Dtos.ProductDto;
using E_Commerce.Core.Dtos;
using MediatR;

namespace E_Commerce.Core.Queries.ProductQueries
{
    public class GetProductsByBrandQuery : IRequest<PaginatedResponse<ProductResponse>>
    {
        public GetProductsByBrandQuery(Guid brandID, PaginationDto pagination)
        {
            BrandID = brandID;
            Pagination = pagination;
        }

        public Guid BrandID { get; }
        public PaginationDto Pagination { get; }

    }
}
