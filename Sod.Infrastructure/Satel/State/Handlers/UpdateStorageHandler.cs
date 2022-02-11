using System;
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
            foreach (var parameter in parameters)
            {
                try
                {
                    await _store.SetAsync(parameter.Key, parameter.Value);
                }
                catch (Exception e)
                {
                    Logger.LogError(e, e.Message);
                }
            }
        }
    }
}