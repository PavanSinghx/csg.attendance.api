namespace CSG.Attendance.Api.Services
{
    public interface IMemoryCacheService
    {
        TValue RetrieveValue<TKey, TValue>(TKey key);
        void SetValue<TKey, TValue>(TKey key, TValue entryValue);
    }
}