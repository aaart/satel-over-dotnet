using System.Threading.Tasks;

namespace Sod.Infrastructure.Store
{
    public interface IStore
    {
        Task AddAsync(string key, object value);
        Task UpdateAsync(string key, object value);
        Task<T> GetAsync<T>(string key);
    }
}