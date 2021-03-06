using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types
{
    public class ActualStateReadTask : SatelTask
    {
        public ActualStateReadTask(string persistedStateKey, NotificationTaskType notificationTaskType, IOReadManipulatorMethod method, OutgoingEventType outgoingEventType)
        {
            PersistedStateKey = persistedStateKey;
            NotificationTaskType = notificationTaskType;
            Method = method;
            OutgoingEventType = outgoingEventType;
        }


        public string PersistedStateKey { get; }
        public NotificationTaskType NotificationTaskType { get; }
        public IOReadManipulatorMethod Method { get; }
        public OutgoingEventType OutgoingEventType { get; }
    }
}