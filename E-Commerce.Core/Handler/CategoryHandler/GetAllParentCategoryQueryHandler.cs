using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos.CategoryDto;
using E_Commerce.Core.Queries.CategoryQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.CategoryHandler
{
    public class GetAllParentCategoryQueryHandler : IRequestHandler<GetAllParentCategoryQuery, IEnumerable<CategoryResponse>>
    {
        private readonly ICategoryService _categoryService;
        private readonly ICacheService _cacheService;

        public GetAllParentCategoryQueryHandler(ICategoryService categoryService, ICacheService cacheService)
        {
            _categoryService = categoryService;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<CategoryResponse>> Handle(GetAllParentCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync($"AllParent", async () =>
            {
                return await _categoryService.GetAllAsync(x => x.ParentCategoryID == null);
            }, cancellationToken);
        }
    }
}
