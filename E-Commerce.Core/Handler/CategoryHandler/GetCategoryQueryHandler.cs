using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos.CategoryDto;
using E_Commerce.Core.Queries.CategoryQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Handler.CategoryHandler
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryResponse>
    {
        private readonly ICategoryService _categoryService;
        private readonly ICacheService _cacheService;

        public GetCategoryQueryHandler(ICategoryService categoryService, ICacheService cacheService)
        {
            _categoryService = categoryService;
            _cacheService = cacheService;
        }

        public async Task<CategoryResponse> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync($"Category-{request.CategoryID}", async () =>
            {
                return await _categoryService.GetByAsync(x=>x.CategoryID == request.CategoryID);
            }, cancellationToken);
        }
    }
}
