using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.State.Events.Outgoing;
using Sod.Infrastructure.Storage;
using Sod.Infrastructure.Storage.TaskTypes.OutputsUpdate;

namespace Sod.Infrastructure.State.Events.Incoming
{
    public class OutputEnqueueUpdateStateHandler : IEventHandler
    {
        private readonly int _ioIndex;
        private readonly ITaskQueue _queue;

        public OutputEnqueueUpdateStateHandler(int ioIndex, ITaskQueue queue)
        {
            _ioIndex = ioIndex;
            _queue = queue;
        }

        public async Task HandleAsync(IncomingEvent incomingEvent)
        {
            var data = new OutputsUpdateTask(
                new List<IOState> { new() { Index = _ioIndex, Value = OnOffParse.ToBoolean(incomingEvent.Payload) } },
                true,
                OutgoingEventType.OutputsStateChanged);
            await _queue.EnqueueAsync(data);
        }
    }
}