using System.Collections.Concurrent;
using System.Threading.Tasks;
using Sod.Infrastructure.Store.Exceptions;

namespace Sod.Infrastructure.Store
{
    public class Store : IStore
    {
        private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

        public Task SetAsync(string key, object value)
        {
            _cache.AddOrUpdate(key, value, (_, _) => value);
            return Task.CompletedTask;
        }

        public Task<T> GetAsync<T>(string key)
        {
            if (!_cache.TryGetValue(key, out object? value))
            {
                throw new KeyNotFoundException();
            }
            
            return Task.FromResult<T>((T)value);
        }

        public Task<bool> ExistsAsync<T>(string key)
        {
            return Task.FromResult(_cache.ContainsKey(key));
        }
    }
}