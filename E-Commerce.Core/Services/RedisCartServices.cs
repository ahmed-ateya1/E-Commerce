using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.ServicesContract;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace E_Commerce.Core.Services
{
    public class RedisCartServices : IRedisCartServices
    {
        private readonly IDatabase _redisDatabase;

        public RedisCartServices(IConnectionMultiplexer redis)
        {
            _redisDatabase = redis.GetDatabase();
        }

        public async Task<bool> DeleteCartAsync(Guid cartId)
        {
            return await _redisDatabase.KeyDeleteAsync(cartId.ToString());
        }

        public async Task<Cart> GetCartAsync(Guid cartId)
        {
           var date = await _redisDatabase.StringGetAsync(cartId.ToString());
           return date.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<Cart>(date);
        }

        public async Task<Cart> UpdateCartAsync(Cart cart)
        {
            var created = await _redisDatabase
                .StringSetAsync(cart.CartID.ToString(), JsonConvert.SerializeObject(cart) , TimeSpan.FromDays(30));
            return created ? cart : null;
        }
    }
}
