using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ProductDto;
using E_Commerce.Core.Queries.ProductQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Handler.ProductHandler
{
    public class GetProductsByPriceRangeQueryHandler : IRequestHandler<GetProductsByPriceRangeQuery, PaginatedResponse<ProductResponse>>
    {
        private readonly ICacheService _cacheService;
        private readonly IProductService _productService;

        public GetProductsByPriceRangeQueryHandler(ICacheService cacheService, IProductService productService)
        {
            _cacheService = cacheService;
            _productService = productService;
        }

        public async Task<PaginatedResponse<ProductResponse>> Handle(GetProductsByPriceRangeQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService
                .GetAsync($"GetProductsByPriceRange{request.PriceRange.Min}{request.PriceRange.Max}{request.PriceRange.Pagination.PageIndex}{request.PriceRange.Pagination.PageSize}{request.PriceRange.Pagination.SortBy}{request.PriceRange.Pagination.SortDirection}"
                , async () =>
                {
                    return await _productService
                    .GetAllAsync(x=>x.ProductPrice>=request.PriceRange.Min 
                    && x.ProductPrice <= request.PriceRange.Max , request.PriceRange.Pagination);
                }, cancellationToken);
        }
    }
}
