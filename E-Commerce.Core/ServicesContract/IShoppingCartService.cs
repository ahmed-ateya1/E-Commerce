using E_Commerce.Core.Domain.Entities;

namespace E_Commerce.Core.ServicesContract
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetCartAsync(string userId, CancellationToken cancellationToken = default);
        Task AddToCartAsync(string userId, CartItems item, CancellationToken cancellationToken = default);
        Task RemoveFromCartAsync(string userId, Guid productId, CancellationToken cancellationToken = default);
        Task ClearCartAsync(string userId, CancellationToken cancellationToken = default);
    }
}
