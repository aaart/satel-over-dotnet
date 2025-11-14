using Microsoft.Extensions.Logging;
using Sod.Model.DataStructures;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types;

public class PersistedStateUpdateTaskHandler(IStore store) : BaseHandler<PersistedStateUpdateTask>
{
    protected override async Task<IEnumerable<SatelTask>> Handle(PersistedStateUpdateTask data)
    {
        try
        {
            await store.SetAsync(data.StorageKey, data.Values);
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
        }

        return Enumerable.Empty<SatelTask>();
    }
}