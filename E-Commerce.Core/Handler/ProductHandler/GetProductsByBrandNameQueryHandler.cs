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
    public class GetProductsByBrandNameQueryHandler : IRequestHandler<GetProductsByBrandNameQuery, PaginatedResponse<ProductResponse>>
    {
        private readonly IProductService _productService;
        private readonly ICacheService _cacheService;

        public GetProductsByBrandNameQueryHandler(IProductService productService, ICacheService cacheService)
        {
            _productService = productService;
            _cacheService = cacheService;
        }

        public async Task<PaginatedResponse<ProductResponse>> Handle(GetProductsByBrandNameQuery request, CancellationToken cancellationToken)
        {
           return await _cacheService
                .GetAsync($"GetProductsByBrandName{request.BrandName}{request.Pagination.PageIndex}{request.Pagination.PageSize}{request.Pagination.SortBy}{request.Pagination.SortDirection}"
                , async () =>
                {
                    return await _productService
                    .GetAllAsync(x => x.Brand.BrandName.ToUpper().Contains(request.BrandName.ToUpper()), request.Pagination);
                }, cancellationToken);
        }
    }
}
