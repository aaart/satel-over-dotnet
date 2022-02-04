using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sod.Infrastructure.Store.Exceptions;
using StackExchange.Redis;

namespace Sod.Infrastructure.Store
{
    public class RedisStore : IStore
    {
        private readonly IDatabaseAsync _database;

        public RedisStore(IDatabaseAsync database)
        {
            _database = database;
        }
        
        public async Task SetAsync(string key, object value)
        {
            var serializeObject = JsonConvert.SerializeObject(value);
            if (!await _database.StringSetAsync(new RedisKey(key), new RedisValue(serializeObject)))
            {
                throw new ValueNotUpdatedException();
            }
        }

        public Task PushAsync(string key, object value)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var redisValue = await _database.StringGetAsync(new RedisKey(key));
            if (!redisValue.HasValue)
            {
                throw new KeyNotFoundException();
            }
            return JsonConvert.DeserializeObject<T>(redisValue.ToString()) ?? throw new NullReferenceException();
        }

        public async Task<bool> ExistsAsync<T>(string key)
        {
            var redisValue = await _database.StringGetAsync(new RedisKey(key));
            return redisValue.HasValue;
        }
    }
}