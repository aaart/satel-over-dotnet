using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sod.Model.DataStructures;

public class InMemoryStore : IStore
{
    private Dictionary<string, object> _store = new();

    public Task SetAsync(string key, object value)
    {
        _store[key] = value;

        return Task.CompletedTask;
    }

    public Task<T> GetAsync<T>(string key)
    {
        return Task.FromResult((T)_store[key]);
    }
}