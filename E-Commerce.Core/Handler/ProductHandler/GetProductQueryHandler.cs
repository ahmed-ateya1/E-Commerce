using E_Commerce.Core.Caching;
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
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductResponse>
    {
        private readonly IProductService _productService;
        private readonly ICacheService _cacheService;

        public GetProductQueryHandler(IProductService productService, ICacheService cacheService)
        {
            _productService = productService;
            _cacheService = cacheService;
        }

        public async Task<ProductResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync<ProductResponse>($"GetProduct{request.Id}", async () =>
            {
                return await _productService.GetByAsync(x=>x.ProductID == request.Id);
            }, cancellationToken);
        }
    }
}
