using Microsoft.Extensions.Caching.Memory;
using Pangea.Shared.Caching.Contracts;
using Pangea.Shared.Caching.Models;

namespace Pangea.Shared.Caching.Services
{
    public class LocalCacheService : ICache
    {
        #region Members

        private readonly CacheSettings _settings;
        private readonly IMemoryCache _memoryCache;

        private static readonly int _defaultDuration = 5;
        private static readonly object _lockObject = new();

        #endregion

        #region Constructors

        public LocalCacheService(IMemoryCache memoryCache, CacheSettings settings)
        {
            _memoryCache = memoryCache;
            _settings = settings;
        }

        #endregion

        #region Methods

        public T? Get<T>(string key, Func<Task<T>>? fetch = null, int? duration = null) where T : class
        {
            if (_memoryCache.TryGetValue(key, out T value)) return value;


            if (fetch != null)
            {
                lock (_lockObject)
                {
                    value = fetch().Result;
                    _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration ?? _settings?.Duration ?? _defaultDuration));
                }

                return value;
            }

            return null;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        #endregion
    }
}
