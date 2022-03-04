using Sod.Model.CommonTypes;

namespace Sod.Model.Tasks.Types
{
    public class ReadStateTask : SatelTask
    {
        public ReadStateTask(string persistedStateKey, NotificationTaskType notificationNotificationTaskType, IOReadManipulatorMethod method, OutgoingEventType outgoingEventType)
        {
            PersistedStateKey = persistedStateKey;
            NotificationNotificationTaskType = notificationNotificationTaskType;
            Method = method;
            OutgoingEventType = outgoingEventType;
        }


        public string PersistedStateKey { get; }
        public NotificationTaskType NotificationNotificationTaskType { get; }
        public IOReadManipulatorMethod Method { get; }
        public OutgoingEventType OutgoingEventType { get; }
    }
}