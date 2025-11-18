using Microsoft.Extensions.Logging;
using Sod.Model.DataStructures;

namespace Sod.Model.Tasks.Handlers.Impl;

public class PersistedStateUpdateTaskHandler(IStore store) : BaseHandler<PersistedStateUpdateTask>
{
    protected override async Task<IEnumerable<BaseSatelTask>> Handle(PersistedStateUpdateTask data)
    {
        try
        {
            await store.SetAsync(data.StorageKey, data.Values);
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
        }

        return Enumerable.Empty<BaseSatelTask>();
    }
}