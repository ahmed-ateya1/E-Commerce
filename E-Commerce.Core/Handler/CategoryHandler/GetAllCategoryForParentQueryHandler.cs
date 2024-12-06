using E_Commerce.Core.Caching;
using E_Commerce.Core.Dtos;
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
    public class GetAllCategoryForParentQueryHandler : IRequestHandler<GetAllCategoryForParentQuery, IEnumerable<CategoryResponse>>
    {
        private readonly ICacheService _cacheService;
        private readonly ICategoryService _categoryService;

        public GetAllCategoryForParentQueryHandler(ICacheService cacheService, ICategoryService categoryService)
        {
            _cacheService = cacheService;
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<CategoryResponse>> Handle(GetAllCategoryForParentQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync("ParentCategories", async () =>
            {
                return await _categoryService.GetAllAsync(x=>x.ParentCategoryID == request.ParentCategoryID);
            }, cancellationToken);
        }
    }
}
