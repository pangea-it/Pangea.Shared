using Pangea.Shared.Caching.Contracts;
using Pangea.Shared.Caching.Models;
using Pangea.Shared.Extensions.General;
using StackExchange.Redis;

namespace Pangea.Shared.Caching.Services
{
    public class DistributedCacheService : ICache
    {
        #region Class members

        private readonly Lazy<ConnectionMultiplexer> _connection;
        private readonly CacheSettings _settings;

        private static readonly int _defaultDuration = 5;
        private static readonly int _defaultPort = 6379;
        private static readonly int _defaultConnectTimeout = 60 * 1000;

        #endregion

        #region Constructors

        public DistributedCacheService(CacheSettings settings)
        {
            _settings = settings;
            _connection = new Lazy<ConnectionMultiplexer>(() => 
            {
                ConfigurationOptions options = new()
                {
                    EndPoints = { { _settings?.DistributedCacheSettings?.Endpoint!, _settings?.DistributedCacheSettings?.Port ?? _defaultPort } },
                    AllowAdmin = _settings?.DistributedCacheSettings?.AllowAdmin ?? true,
                    ConnectTimeout = _settings?.DistributedCacheSettings?.ConnectTimeout ?? _defaultConnectTimeout
                };

                return ConnectionMultiplexer.Connect(options);
            });
        }

        #endregion


        public T? Get<T>(string key, Func<Task<T>>? fetch = null, int? duration = null) where T : class
        {
            var cacheStorage = _connection.Value.GetDatabase();

            var result = cacheStorage.StringGet(key);
            if (result.HasValue) return result.ToString().FromJson<T>();

            if (fetch != null)
            {
                var value = fetch().Result;
                cacheStorage.StringSet(key, value.ToJson(), TimeSpan.FromMinutes(duration ?? _settings?.Duration ?? _defaultDuration), When.Always);

                return value;
            }

            return null;
        }

        public void Remove(string key)
        {
            var cacheStorage = _connection.Value.GetDatabase();
            cacheStorage.KeyExpire(key, TimeSpan.FromMilliseconds(100f));
        }
    }
}
