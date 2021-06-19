using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public TValue RetrieveValue<TKey, TValue>(TKey key)
        {
            this.memoryCache.TryGetValue(key, out TValue cachedEntry);
            return cachedEntry;
        }

        public void SetValue<TKey, TValue>(TKey key, TValue entryValue)
        {
            this.memoryCache.Set(key, entryValue);
        }

        public Task<TValue> GetOrCreateAsync<TKey, TValue>(TKey key, Func<ICacheEntry, Task<TValue>> factory)
        {
            return this.memoryCache.GetOrCreateAsync(key, factory);
        }
    }
}
