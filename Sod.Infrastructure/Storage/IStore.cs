using System.Threading.Tasks;

namespace Sod.Infrastructure.Storage
{
    public interface IStore
    {
        Task SetAsync(string key, object value);
        Task<T> GetAsync<T>(string key);
    }
}