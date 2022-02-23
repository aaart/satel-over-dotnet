using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Handlers.StorageUpdate
{
    public class UpdateStorageStateHandler : LoggingCapability, IStateHandler
    {
        private readonly IStore _store;

        public UpdateStorageStateHandler(IStore store)
        {
            _store = store;
        }
        
        public async Task<IEnumerable<SatelTask>> Handle(IReadOnlyDictionary<string, object> parameters)
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

            return Enumerable.Empty<SatelTask>();
        }
    }
}