using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.CategoryDto;
using E_Commerce.Core.Dtos;
using System.Linq.Expressions;

namespace E_Commerce.Core.ServicesContract
{
    public interface ICategoryService
    {
        Task<CategoryResponse?> CreateAsync(CategoryAddRequest? request);
        Task<CategoryResponse> UpdateAsync(CategoryUpdateRequest? request);
        Task<bool> DeleteAsync(Guid id);
        Task<CategoryResponse?> GetByAsync(Expression<Func<Category, bool>> predict, bool isTracked = false);
        Task<IEnumerable<CategoryResponse>> GetAllAsync(Expression<Func<Category, bool>>? predicate = null, PaginationDto? pagination = null);
    }
}
