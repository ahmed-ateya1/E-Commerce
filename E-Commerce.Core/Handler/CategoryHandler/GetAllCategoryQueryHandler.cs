﻿using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos.CategoryDto;
using E_Commerce.Core.Queries.CategoryQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.CategoryHandler
{
    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, IEnumerable<CategoryResponse>>
    {
        private readonly ICacheService _cacheService;
        private readonly ICategoryService _categoryService;

        public GetAllCategoryQueryHandler(ICacheService cacheService,
            ICategoryService categoryService)
        {
            _cacheService = cacheService;
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<CategoryResponse>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {

            return await _cacheService.GetAsync($"Categories", async () =>
            {
                return await _categoryService.GetAllAsync(request.Filter);
            }, cancellationToken);
        }
    }
}
