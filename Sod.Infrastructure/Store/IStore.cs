using System.Threading.Tasks;

namespace Sod.Infrastructure.Store
{
    public interface IStore
    {
        Task SetAsync(string key, object value);
        Task PushAsync(string key, object value);
        Task<T> GetAsync<T>(string key);
        Task<bool> ExistsAsync<T>(string key);
    }
}