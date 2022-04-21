using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Model.DataStructures;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types
{
    public class PersistedStateUpdateTaskHandler : BaseHandler<PersistaedStateUpdateTask>
    {
        private readonly IStore _store;

        public PersistedStateUpdateTaskHandler(IStore store)
        {
            _store = store;
        }

        protected override async Task<IEnumerable<SatelTask>> Handle(PersistaedStateUpdateTask data)
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