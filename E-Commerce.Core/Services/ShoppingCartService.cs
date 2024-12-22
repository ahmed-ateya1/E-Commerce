using E_Commerce.Core.Caching;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.ServicesContract;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace E_Commerce.Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMemoryCacheService _memoryCache;
        private readonly ILogger<ShoppingCartService> _logger;
        private const string CartKeyPrefix = "ShoppingCart_";


        public ShoppingCartService(IMemoryCacheService memoryCache, ILogger<ShoppingCartService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }
        private string GetCartKey(string userId) => $"{CartKeyPrefix}{userId}";

        public async Task<ShoppingCart> GetCartAsync(string userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting cart for user {UserId}", userId);
            string cartKey = GetCartKey(userId.ToString());
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("User ID is null or empty");
                return new ShoppingCart();
            }
            _logger.LogInformation("Getting cart from cache for user {UserId}", userId);
            return await _memoryCache.GetByAsync<ShoppingCart>(cartKey, cancellationToken)
                ?? new ShoppingCart();
        }

        public async Task AddToCartAsync(string userId, CartItems item, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Adding item to cart for user {UserId}", userId);
            string cartKey = GetCartKey(userId);

            _logger.LogInformation("Getting cart from cache for user {UserId}", userId);
            var cart = await GetCartAsync(userId, cancellationToken);

            _logger.LogInformation("Adding item to cart for user {UserId}", userId);
            var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductID == item.ProductID);
            if (existingItem != null)
            {
                _logger.LogInformation("Item already exists in cart for user {UserId}", userId);
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                _logger.LogInformation("Item does not exist in cart for user {UserId}", userId);
                cart.CartItems.Add(item);
            }
            _logger.LogInformation("Setting cart in cache for user {UserId}", userId);
            await _memoryCache.SetAsync(cartKey, cart, cancellationToken);
        }

        public async Task RemoveFromCartAsync(string userId, Guid productId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Removing item from cart for user {UserId}", userId);
            string cartKey = GetCartKey(userId);
            _logger.LogInformation("Getting cart from cache for user {UserId}", userId);

            var cart = await GetCartAsync(userId, cancellationToken);
            _logger.LogInformation("Removing item from cart for user {UserId}", userId);

            cart.CartItems.RemoveAll(i => i.ProductID == productId);
            _logger.LogInformation("Setting cart in cache for user {UserId}", userId);

            await _memoryCache.SetAsync(cartKey, cart, cancellationToken);
        }

        public async Task ClearCartAsync(string userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Clearing cart for user {UserId}", userId);
            string cartKey = GetCartKey(userId);
            _logger.LogInformation("Removing cart from cache for user {UserId}", userId);
            await _memoryCache.RemoveAsync(cartKey, cancellationToken);
        }
    }
}
