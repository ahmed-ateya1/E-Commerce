using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ProductDto;
using E_Commerce.Core.Queries.ProductQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.ProductHandler
{
    public class GetProductByParentCategoryQueryHandler : IRequestHandler<GetProductByParentCategoryQuery, PaginatedResponse<ProductResponse>>
    {
        private readonly IProductService _productService;
        private readonly ICacheService _cacheService;

        public GetProductByParentCategoryQueryHandler(IProductService productService, ICacheService cacheService)
        {
            _productService = productService;
            _cacheService = cacheService;
        }

        public async Task<PaginatedResponse<ProductResponse>> Handle(GetProductByParentCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync($"GetProductByParentCategory{request.ParentCategoryID}{request.Pagination.PageIndex}{request.Pagination.PageSize}{request.Pagination.SortBy}{request.Pagination.SortDirection}",
                async () =>
                {
                    return await _productService.GetAllAsync(x => x.Category.ParentCategoryID == request.ParentCategoryID, request.Pagination);
                }, cancellationToken);
        }
    }
}
