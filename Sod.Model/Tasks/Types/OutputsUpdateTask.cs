using System.Collections.Generic;
using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Types
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