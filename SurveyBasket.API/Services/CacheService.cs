using Microsoft.Extensions.Caching.Distributed;
using SurveyBasket.API.Repositories;
using System.Text.Json;

namespace SurveyBasket.API.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IDistributedCache distributedCache, ILogger<CacheService> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            _logger.LogInformation("Get Cache With Key: {Key}", key);
            var cacheValue = await _distributedCache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(cacheValue))
                return null;

            return JsonSerializer.Deserialize<T>(cacheValue);
        }
        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            _logger.LogInformation("Set Cache With Key: {Key}", key);
            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), cancellationToken);
        }
        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Remove Cache With Key: {Key}", key);
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
    }
}
