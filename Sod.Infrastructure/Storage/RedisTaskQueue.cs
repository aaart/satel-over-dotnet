using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Sod.Infrastructure.Storage
{
    public class RedisTaskQueue : ITaskQueue
    {
        private readonly RedisKey _key = new(nameof(RedisTaskQueue));
        private readonly IDatabaseAsync _database;

        public RedisTaskQueue(IDatabaseAsync database)
        {
            _database = database;
        }

        public async Task EnqueueAsync(SatelTask satelTask) => await _database.ListRightPushAsync(_key, new RedisValue(JsonConvert.SerializeObject(satelTask)));

        public async Task<(bool exists, SatelTask? value)> DequeueAsync()
        {
            var head = await _database.ListLeftPopAsync(_key);
            return (head.HasValue, head.HasValue ? JsonConvert.DeserializeObject<SatelTask>(head.ToString()) : null);
        }
    }
}