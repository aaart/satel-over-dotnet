using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel;
using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Events.Incoming.Events.Handlers
{
    public class PairedOutputEnqueueStateUpdateHandler : IEventHandler
    {
        private readonly int _ioIndex;
        private readonly ITaskQueue _queue;

        public PairedOutputEnqueueStateUpdateHandler(int ioIndex, ITaskQueue queue)
        {
            _ioIndex = ioIndex;
            _queue = queue;
        }
        
        public async Task HandleAsync(IncomingEvent incomingEvent)
        {
            var data = new OutputsUpdateTask(
                new List<IOState> { new() { Index = _ioIndex, Value = OnOffParse.ToBoolean(incomingEvent.Payload) } },
                false,
                OutgoingEventType.OutputsStateChanged);
            await _queue.EnqueueAsync(data);
        }
    }
}