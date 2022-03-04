using System;
using System.Collections.Generic;
using Sod.Infrastructure.State.Events.Outgoing;

namespace Sod.Infrastructure.Storage.TaskTypes.OutputsUpdate
{
    public class OutputsUpdateTask : SatelTask
    {
        public OutputsUpdateTask(IEnumerable<IOState> updates, bool notifyChanged, OutgoingEventType eventType)
        {
            Updates = updates;
            NotifyChanged = notifyChanged;
            EventType = eventType;
        }

        public IEnumerable<IOState> Updates { get; }
        public bool NotifyChanged { get; }
        public OutgoingEventType EventType { get; }
    }
}