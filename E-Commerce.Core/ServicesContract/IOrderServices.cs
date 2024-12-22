using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.OrderDto;
using E_Commerce.Core.Helper;
using System.Linq.Expressions;

namespace E_Commerce.Core.ServicesContract
{
    public interface IOrderServices
    {
        Task<ServiceResponse> CreateAsync(OrderAddRequest? orderRequest);
        Task<ServiceResponse> UpdateAsync(Guid orderID , OrderStatus orderStatus);
        Task<ServiceResponse> DeleteAsync(Guid id);
        Task<ServiceResponse> GetByAsync(Expression<Func<Order, bool>> filter, bool isTracked = false);
        Task<ServiceResponse> GetAllAsync(Expression<Func<Order, bool>>? filter = null);
    }
}
