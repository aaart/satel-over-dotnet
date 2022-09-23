using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Handlers.Policies;

public class ActualStateAlarmArmedPostReadPolicy : IPostReadPolicy
{
    private readonly IStore _store;

    public ActualStateAlarmArmedPostReadPolicy(IStore store)
    {
        _store = store;
    }
    
    public IEnumerable<SatelTask> Apply(IList<IOState> changes, string persistedStateKey, bool[] actualState, OutgoingEventType outgoingEventType)
    {
        throw new NotImplementedException();
    }
}