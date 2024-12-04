using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Caching
{
    /// <summary>
    /// Interface for cache service operations.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Gets a cached value by key.
        /// </summary>
        /// <typeparam name="T">Type of the cached value.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The cached value or null if not found.</returns>
        Task<T> GetByAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Gets a cached value or generates it using a factory method if not found.
        /// </summary>
        /// <typeparam name="T">Type of the cached value.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="factory">The factory method to generate the value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The cached or generated value.</returns>
        Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Sets a value in the cache with the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The value to cache.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The cached value.</returns>
        Task<T> SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Removes a cached value by key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes all cache entries with keys starting with the specified prefix.
        /// </summary>
        /// <param name="prefix">The key prefix.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task RemoveByPrefix(string prefix, CancellationToken cancellationToken = default);
    }
}
