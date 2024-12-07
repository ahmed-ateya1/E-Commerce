using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using System.Linq.Expressions;

namespace E_Commerce.Core.ServicesContract
{
    public interface ITechnicalSpecificationService
    {
        Task<TechnicalSpecificationResponse> CreateAsync(TechnicalSpecificationAddRequest? request);
        Task<TechnicalSpecificationResponse> UpdateAsync(TechnicalSpecificationUpdateRequest? request);
        Task<bool> DeleteAsync(Guid id);
        Task<TechnicalSpecificationResponse?> GetByAsync(Expression<Func<TechnicalSpecification, bool>> expression, bool isTracked = false);
        Task<IEnumerable<TechnicalSpecificationResponse>> GetAllAsync(Expression<Func<TechnicalSpecification, bool>>? expression = null);
    }
}
