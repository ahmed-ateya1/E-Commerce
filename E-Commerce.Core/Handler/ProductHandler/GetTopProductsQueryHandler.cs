using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos.ProductDto;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Queries.ProductQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.ProductHandler
{
    public class GetTopProductsQueryHandler : IRequestHandler<GetTopProductsQuery, PaginatedResponse<ProductResponse>>
    {
        private readonly ICacheService _cacheService;
        private readonly IProductService _productService;

        public GetTopProductsQueryHandler(ICacheService cacheService, IProductService productService)
        {
            _cacheService = cacheService;
            _productService = productService;
        }

        public async Task<PaginatedResponse<ProductResponse>> Handle(GetTopProductsQuery request, CancellationToken cancellationToken)
        {
            request.Pagination.SortBy = "TotalOrders";
            request.Pagination.SortDirection = "desc";
            return await _cacheService.GetAsync(
                $"GetTopProductsQueryHandler_GetTopProductsQueryHandler_{request.Pagination.PageIndex}_{request.Pagination.PageSize}",
                async () => await _productService.GetAllAsync(pagination: request.Pagination),
                cancellationToken);
        }
    }
}
