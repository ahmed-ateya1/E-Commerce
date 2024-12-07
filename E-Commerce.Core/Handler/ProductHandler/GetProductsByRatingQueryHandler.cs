using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ProductDto;
using E_Commerce.Core.Queries.ProductQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.ProductHandler
{
    public class GetProductsByRatingQueryHandler : IRequestHandler<GetProductsByRatingQuery, PaginatedResponse<ProductResponse>>
    {
        private readonly ICacheService _cacheService;
        private readonly IProductService _productService;

        public GetProductsByRatingQueryHandler(ICacheService cacheService, IProductService productService)
        {
            _cacheService = cacheService;
            _productService = productService;
        }

        public async Task<PaginatedResponse<ProductResponse>> Handle(GetProductsByRatingQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService
               .GetAsync($"GetProductsByRating{request.Rating}{request.Pagination.PageIndex}{request.Pagination.PageSize}{request.Pagination.SortBy}{request.Pagination.SortDirection}"
               , async () =>
               {
                   return await _productService.GetAllAsync(x => x.AvgRating == request.Rating, request.Pagination);
               }, cancellationToken);
        }
    }
}
