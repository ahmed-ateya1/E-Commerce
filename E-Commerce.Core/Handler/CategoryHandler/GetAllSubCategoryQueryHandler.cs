using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos.CategoryDto;
using E_Commerce.Core.Queries.CategoryQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;

namespace E_Commerce.Core.Handler.CategoryHandler
{
    public class GetAllSubCategoryQueryHandler : IRequestHandler<GetAllSubCategoryQuery, IEnumerable<CategoryResponse>>
    {
        private readonly ICacheService _cacheService;
        private readonly ICategoryService _categoryService;

        public GetAllSubCategoryQueryHandler(ICacheService cacheService, ICategoryService categoryService)
        {
            _cacheService = cacheService;
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<CategoryResponse>> Handle(GetAllSubCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync($"AllSubCategories", async () =>
            {
                return await _categoryService.GetAllAsync(x => x.ParentCategoryID != null);
            }, cancellationToken);
        }
    }
}
