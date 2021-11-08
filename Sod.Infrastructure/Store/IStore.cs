using System.Threading.Tasks;

namespace Sod.Infrastructure.Store
{
    public interface IStore
    {
        Task AddAsync(string key, object value);
        Task GetAsync<T>(string key);
    }
}