using Sod.Model.CommonTypes;

namespace Sod.Model.Tasks.Types
{
    public class ReadStateTask : SatelTask
    {
        public ReadStateTask(string persistedStateKey, TaskType notificationTaskType, IOReadManipulatorMethod method, OutgoingEventType outgoingEventType)
        {
            PersistedStateKey = persistedStateKey;
            NotificationTaskType = notificationTaskType;
            Method = method;
            OutgoingEventType = outgoingEventType;
        }


        public string PersistedStateKey { get; }
        public TaskType NotificationTaskType { get; }
        public IOReadManipulatorMethod Method { get; }
        public OutgoingEventType OutgoingEventType { get; }
    }
}