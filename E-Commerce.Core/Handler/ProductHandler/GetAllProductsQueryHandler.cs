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
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginatedResponse<ProductResponse>>
    {
        private readonly ICacheService _cacheService;
        private readonly IProductService _productService;

        public GetAllProductsQueryHandler(ICacheService cacheService, IProductService productService)
        {
            _cacheService = cacheService;
            _productService = productService;
        }

        public async Task<PaginatedResponse<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService
               .GetAsync<PaginatedResponse<ProductResponse>>($"GetAllProducts{request.Pagination.PageIndex}{request.Pagination.PageSize}{request.Pagination.SortBy}{request.Pagination.SortDirection}"
               , async () =>
               {
                   return await _productService.GetAllAsync(null,request.Pagination);
               },cancellationToken);
        }
    }
}
