using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ProductDto;
using E_Commerce.Core.Queries.ProductQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;
using System.Linq.Expressions;

namespace E_Commerce.Core.Handler.ProductHandler
{
    /// <summary>
    /// Handles the retrieval of products by discount using caching and product services.
    /// </summary>
    public class GetProductsByDiscountQueryHandler : IRequestHandler<GetProductsByDiscountQuery, PaginatedResponse<ProductResponse>>
    {
        private readonly ICacheService _cacheService;
        private readonly IProductService _productService;

        public GetProductsByDiscountQueryHandler(ICacheService cacheService, IProductService productService)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        /// <summary>
        /// Handles the query to fetch products by discount with pagination support.
        /// </summary>
        /// <param name="request">The request containing discount and pagination parameters.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A paginated response of products meeting the discount criteria.</returns>
        public async Task<PaginatedResponse<ProductResponse>> Handle(GetProductsByDiscountQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            string cacheKey = GenerateCacheKey(request);

            return await _cacheService.GetAsync(
                cacheKey,
                async () =>
                {
                    Expression<Func<Product, bool>> filter = request.Discount.HasValue
                        ? x => x.Discount >= request.Discount
                        : x => x.Discount > 0;

                    return await _productService.GetAllAsync(filter, request.Pagination);
                },
                cancellationToken);
        }


        /// <summary>
        /// Generates a unique cache key based on the request parameters.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <returns>A unique string representing the cache key.</returns>
        private static string GenerateCacheKey(GetProductsByDiscountQuery request)
        {
            return $"GetProductsByDiscount-{request.Pagination.PageIndex}-{request.Pagination.PageSize}-{request.Discount?.ToString() ?? "Any"}";
        }
    }
}
