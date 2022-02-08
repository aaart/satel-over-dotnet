using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public class UpdateStorageHandler : LoggingCapability, IHandler
    {
        private readonly IStore _store;

        public UpdateStorageHandler(IStore store)
        {
            _store = store;
        }
        
        public async Task Handle(IReadOnlyDictionary<string, object> parameters)
        {
            foreach (var key in parameters.Keys)
            {
                await _store.SetAsync(key, parameters[key]);
            }
        }
    }
}