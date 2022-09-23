using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Handlers.Policies;

public interface IPostReadPolicy
{
    IEnumerable<SatelTask> Apply(IList<IOState> changes, string persistedStateKey, bool[] actualState, OutgoingEventType outgoingEventType);
}