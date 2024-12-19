using E_Commerce.Core.Dtos.OrderDto;
using E_Commerce.Core.Helper;
using System.Linq.Expressions;

namespace E_Commerce.Core.ServicesContract
{
    public interface IOrderServices
    {
        Task<OrderResponse> CreateAsync(OrderAddRequest? orderRequest);
        Task<bool> UpdateAsync(Guid orderID , OrderStatus orderStatus);
        Task<bool> DeleteAsync(Guid id);
        Task<OrderResponse> GetByAsync(Expression<Func<Order, bool>> filter, bool isTracked = false);
        Task<IEnumerable<OrderResponse>> GetAllAsync(Expression<Func<Order, bool>>? filter = null);
    }
}
