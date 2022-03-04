using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Storage;
using Sod.Infrastructure.Storage.TaskTypes.StorageUpdate;

namespace Sod.Infrastructure.State.Tasks.Handlers.StorageUpdate
{
    public class UpdateStorageTaskHandler : BaseHandler<StorageUpdateTask>
    {
        private readonly IStore _store;

        public UpdateStorageTaskHandler(IStore store)
        {
            _store = store;
        }

        protected override async Task<IEnumerable<SatelTask>> Handle(StorageUpdateTask data)
        {
            try
            {
                await _store.SetAsync(data.StorageKey, data.Values);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
            }

            return Enumerable.Empty<SatelTask>();
        }
    }
}