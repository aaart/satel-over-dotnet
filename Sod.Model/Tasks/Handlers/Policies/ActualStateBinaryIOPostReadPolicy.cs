using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Policies;

public class ActualStateBinaryIOPostReadPolicy : LoggingCapability, IPostReadPolicy
{
    public virtual IEnumerable<SatelTask> Apply(IList<IOState> changes, string persistedStateKey, bool[] actualState, OutgoingEventType outgoingEventType)
    {
        if (changes.Any())
        {
            Logger.LogInformation($"{changes.Count} changes found. {outgoingEventType} event will be send.");
            var t1 = new PersistedStateUpdateTask(persistedStateKey, actualState);
            var t2 = new ActualStateChangedNotificationTask(changes, outgoingEventType); 
            return new SatelTask[] { t1, t2 };
        }
        
        return Enumerable.Empty<SatelTask>();
    }
}