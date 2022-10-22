﻿using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types;

public class ActualStateBinaryIOReadTask : SatelTask
{
    public ActualStateBinaryIOReadTask(string persistedStateKey, NotificationTaskType notificationTaskType, IOBinaryReadManipulatorMethod method, OutgoingEventType outgoingEventType)
    {
        PersistedStateKey = persistedStateKey;
        NotificationTaskType = notificationTaskType;
        Method = method;
        OutgoingEventType = outgoingEventType;
    }


    public string PersistedStateKey { get; }
    public NotificationTaskType NotificationTaskType { get; }
    public IOBinaryReadManipulatorMethod Method { get; }
    public OutgoingEventType OutgoingEventType { get; }
}