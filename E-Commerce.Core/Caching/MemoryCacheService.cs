using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace E_Commerce.Core.Caching
{
    /// <summary>
    /// Service for in-memory caching operations.
    /// </summary>
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheService> _logger;
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public MemoryCacheService(IMemoryCache memoryCache, ILogger<CacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<T> GetByAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                return _memoryCache.TryGetValue(key, out T value) ? value : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cache for key {Key}", key);
                return null;
            }
        }

        public async Task<T> SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");

            try
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2),
                    SlidingExpiration = TimeSpan.FromSeconds(1)
                };

                _memoryCache.Set(key, value, cacheOptions);
                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache for key {Key}", key);
                throw;
            }
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                _memoryCache.Remove(key);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache for key {Key}", key);
                throw;
            }
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
        {
            if (!_locks.TryGetValue(key, out var semaphore))
            {
                semaphore = new SemaphoreSlim(1, 1);
                _locks[key] = semaphore;
            }

            await semaphore.WaitAsync(cancellationToken);
            try
            {
                var cacheValue = await GetByAsync<T>(key, cancellationToken);
                if (cacheValue != null)
                {
                    return cacheValue;
                }

                cacheValue = await factory();
                await SetAsync(key, cacheValue, cancellationToken);
                return cacheValue;
            }
            finally
            {
                semaphore.Release();
                _locks.TryRemove(key, out _);
            }
        }
    }
}
