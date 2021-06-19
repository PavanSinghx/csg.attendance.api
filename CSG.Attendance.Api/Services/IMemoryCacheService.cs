using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Services
{
    public interface IMemoryCacheService
    {
        TValue RetrieveValue<TKey, TValue>(TKey key);
        void SetValue<TKey, TValue>(TKey key, TValue entryValue);
        Task<TValue> GetOrCreateAsync<TKey, TValue>(TKey key, Func<ICacheEntry, Task<TValue>> factory);
    }
}