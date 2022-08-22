namespace Pangea.Shared.Caching.Contracts
{
    public interface ICache
    {
        T? Get<T>(string key, Func<Task<T>>? fetch = null, int? duration = null) where T : class;
        void Remove(string key);
    }
}
