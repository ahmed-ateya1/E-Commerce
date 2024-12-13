using E_Commerce.Core.Domain.Entities;

namespace E_Commerce.Core.ServicesContract
{
    public interface IRedisCartServices
    {
        Task<Cart> GetCartAsync(Guid cartId);
        Task<Cart> UpdateCartAsync(Cart cart);
        Task<bool> DeleteCartAsync(Guid cartId);

    }
}
