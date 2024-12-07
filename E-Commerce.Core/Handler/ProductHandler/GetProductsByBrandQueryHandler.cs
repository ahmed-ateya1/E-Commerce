using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ProductDto;
using E_Commerce.Core.Queries.ProductQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.ProductHandler
{
    public class GetProductsByBrandQueryHandler : IRequestHandler<GetProductsByBrandQuery, PaginatedResponse<ProductResponse>>
    {
        private readonly IProductService _productService;
        private readonly ICacheService _cacheService;

        public GetProductsByBrandQueryHandler
            (
            IProductService productService, 
            ICacheService cacheService
            )
        {
            _productService = productService;
            _cacheService = cacheService;
        }

        public async Task<PaginatedResponse<ProductResponse>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService
                .GetAsync($"GetProductsByBrand{request.BrandID}{request.Pagination.PageIndex}{request.Pagination.PageSize}{request.Pagination.SortBy}{request.Pagination.SortDirection}"
                , async () =>
                {
                    return await _productService
                    .GetAllAsync(x => x.BrandID == request.BrandID, request.Pagination);
                }, cancellationToken);
        }
    }
}
