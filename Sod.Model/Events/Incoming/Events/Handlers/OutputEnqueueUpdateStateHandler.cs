using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel;
using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Events.Incoming.Events.Handlers
{
    public class OutputEnqueueUpdateStateHandler : LoggingCapability, IEventHandler
    {
        private readonly int _ioIndex;
        private readonly bool _notify;
        private readonly ITaskQueue _queue;

        public OutputEnqueueUpdateStateHandler(int ioIndex, bool notify, ITaskQueue queue)
        {
            _ioIndex = ioIndex;
            _notify = notify;
            _queue = queue;
        }

        public async Task HandleAsync(IncomingEvent incomingEvent)
        {
            Logger.LogInformation($"Event receiver from {incomingEvent.Topic}. Payload: {incomingEvent.Payload}");
            var data = new ActualStateOutputsUpdateTask(
                new List<IOState> { new() { Index = _ioIndex, Value = OnOffParse.ToBoolean(incomingEvent.Payload) } },
                _notify,
                OutgoingEventType.OutputsStateChanged);
            await _queue.EnqueueAsync(data);
        }
    }
}