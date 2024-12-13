using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.ServicesContract;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace E_Commerce.Core.Services
{
    /// <summary>
    /// Provides services for interacting with the shopping cart stored in Redis.
    /// Implements the <see cref="IRedisCartServices"/> interface to manage cart data.
    /// </summary>
    public class RedisCartServices : IRedisCartServices
    {
        private readonly IDatabase _redisDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCartServices"/> class.
        /// </summary>
        /// <param name="redis">The Redis connection multiplexer used to interact with Redis database.</param>
        /// <remarks>
        /// The <paramref name="redis"/> parameter is typically injected via dependency injection in an ASP.NET Core application.
        /// </remarks>
        public RedisCartServices(IConnectionMultiplexer redis)
        {
            _redisDatabase = redis.GetDatabase();
        }

        /// <summary>
        /// Deletes a shopping cart from Redis by its unique identifier.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to be deleted.</param>
        /// <returns>
        /// Returns <c>true</c> if the cart was successfully deleted from Redis; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// If the cart does not exist, this method will return <c>false</c>.
        /// </remarks>
        public async Task<bool> DeleteCartAsync(Guid cartId)
        {
            return await _redisDatabase.KeyDeleteAsync(cartId.ToString());
        }

        /// <summary>
        /// Retrieves a shopping cart from Redis using its unique identifier.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to be retrieved.</param>
        /// <returns>
        /// Returns the <see cref="Cart"/> object if found; otherwise, returns <c>null</c>.
        /// </returns>
        /// <remarks>
        /// If the cart does not exist or the stored data is invalid, the method will return <c>null</c>.
        /// </remarks>
        public async Task<Cart> GetCartAsync(Guid cartId)
        {
            var data = await _redisDatabase.StringGetAsync(cartId.ToString());
            return data.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<Cart>(data);
        }

        /// <summary>
        /// Updates or creates a shopping cart in Redis.
        /// </summary>
        /// <param name="cart">The <see cref="Cart"/> object to be stored or updated in Redis.</param>
        /// <returns>
        /// Returns the updated or created <see cref="Cart"/> object if the operation was successful;
        /// otherwise, returns <c>null</c>.
        /// </returns>
        /// <remarks>
        /// The cart will be serialized and stored in Redis with an expiration time of 30 days.
        /// </remarks>
        public async Task<Cart> UpdateCartAsync(Cart cart)
        {
            var created = await _redisDatabase
                .StringSetAsync(cart.CartID.ToString(), JsonConvert.SerializeObject(cart), TimeSpan.FromDays(30));
            return created ? cart : null;
        }
    }
}
