using Microsoft.Extensions.Caching.Distributed;
using UniAttend.Core.Interfaces.Services;

namespace UniAttend.Infrastructure.Services
{
    public class RateLimiter : IRateLimiter
    {
        private readonly IDistributedCache _cache;

        public RateLimiter(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<bool> CheckAsync(string key, int maxAttempts, TimeSpan window)
        {
            var attempts = await _cache.GetAsync(key);
            if (attempts == null)
            {
                await _cache.SetAsync(key, BitConverter.GetBytes(1), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = window
                });
                return true;
            }

            var count = BitConverter.ToInt32(attempts);
            if (count >= maxAttempts)
                return false;

            await _cache.SetAsync(key, BitConverter.GetBytes(count + 1), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = window
            });
            return true;
        }
    }
}