using System.Threading.Tasks;

namespace Sod.Model.DataStructures;

public interface IStore
{
    Task SetAsync(string key, object value);
    Task<T> GetAsync<T>(string key);
}