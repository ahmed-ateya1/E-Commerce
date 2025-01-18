using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.DealDto;
using E_Commerce.Core.Dtos.ProductDto;
using System.Linq.Expressions;

namespace E_Commerce.Core.ServicesContract
{
    public interface IDealService
    {
        Task<ServiceResponse> CreateAsync(DealAddRequest? request);
        Task<ServiceResponse> UpdateAsync(DealUpdateRequest? request);
        Task<ServiceResponse> DeleteAsync(Guid id);
        Task<ServiceResponse> GetByAsync(Expression<Func<Deal,bool>> expression, bool isTracking = false);
        Task<PaginatedResponse<ProductResponse>> GetAllAsync(Expression<Func<Deal,bool>>? expression = null ,PaginationDto? pagination = null);
    }
}
