using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.DeliveryMethodDto;
using System.Linq.Expressions;

namespace E_Commerce.Core.ServicesContract
{
    public interface IDeliveryMethodServices
    {
        Task<DeliveryMethodResponse> CreateAsync(DeliveryMethodAddRequest? request);
        Task<DeliveryMethodResponse?> UpdateAsync(DeliveryMethodUpdateRequest? request);
        Task<bool> DeleteAsync(Guid id);
        Task<DeliveryMethodResponse?> GetByAsync(Expression<Func<DeliveryMethod , bool>> expression);
        Task<IEnumerable<DeliveryMethodResponse>> GetAllAsync(Expression<Func<DeliveryMethod, bool>>? expression = null);
    }
}
