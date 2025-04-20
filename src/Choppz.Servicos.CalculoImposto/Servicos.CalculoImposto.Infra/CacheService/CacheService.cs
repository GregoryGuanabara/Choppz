using Microsoft.Extensions.Caching.Memory;
using Servicos.CalculoImposto.Core.Abstractions.CacheService;

namespace Servicos.CalculoImposto.Infra.Service.Cache
{
    public sealed class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T? Pegar<T>(string key)
        {
            if (_cache.TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;

            return default(T);
        }

        public void Inserir<T>(string key, T value, TimeSpan expiration)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expiration);

            _cache.Set(key, value, cacheOptions);
        }

        public void Remover(string key)
        {
            _cache.Remove(key);
        }

        public bool Existe(string key)
        {
            return _cache.TryGetValue(key, out _);
        }
    }
}