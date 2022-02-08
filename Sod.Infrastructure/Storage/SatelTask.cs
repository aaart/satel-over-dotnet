using System.Collections.Generic;
using Newtonsoft.Json;
using Sod.Infrastructure.Enums;

namespace Sod.Infrastructure.Storage
{
    public class SatelTask
    {
        public SatelTask(TaskType type)
            : this(type, new Dictionary<string, object>())
        {
        }
        
        [JsonConstructor]
        public SatelTask(TaskType type, IReadOnlyDictionary<string, object> parameters)
        {
            Type = type;
            Parameters = parameters;
        }
        
        public TaskType Type { get; }
        public IReadOnlyDictionary<string, object> Parameters { get; }
    }
}