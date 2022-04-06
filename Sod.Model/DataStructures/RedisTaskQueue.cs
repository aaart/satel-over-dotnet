using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sod.Model.Tasks;
using StackExchange.Redis;

namespace Sod.Model.DataStructures
{
    public class RedisTaskQueue : ITaskQueue
    {
        private readonly RedisKey _key = new(Constants.Queue.RedisTaskQueue);
        private readonly IDatabaseAsync _database;

        public RedisTaskQueue(IDatabaseAsync database)
        {
            _database = database;
        }

        public async Task EnqueueAsync(SatelTask satelTask) => 
            await _database.ListRightPushAsync(_key, new RedisValue(JsonConvert.SerializeObject(new { Type = satelTask.GetType().FullName, Object = satelTask })));

        public async Task<(bool exists, SatelTask? value)> DequeueAsync()
        {
            var head = await _database.ListLeftPopAsync(_key);
            if (!head.HasValue)
            {
                return (false, null);
            }
            
            var persistedValue = (JObject)JsonConvert.DeserializeObject(head.ToString())!;
            var typeName = persistedValue.Value<string>("Type")!;
            var type = Type.GetType(typeName)!;
            var serialized = persistedValue.GetValue("Object")!.ToString()!;
            var deserialized = (SatelTask)JsonConvert.DeserializeObject(serialized, type)!;
            return (true, deserialized);
        }

    }
}