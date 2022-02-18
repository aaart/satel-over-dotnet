using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Events.Incoming
{
    public class OutputEnqueueUpdateStateHandler : IEventHandler
    {
        private readonly string _ioIndex;
        private readonly ITaskQueue _queue;

        public OutputEnqueueUpdateStateHandler(int ioIndex, ITaskQueue queue)
        {
            _ioIndex = Convert.ToString(ioIndex);
            _queue = queue;
        }

        public async Task HandleAsync(IncomingEvent incomingEvent)
        {
            await _queue.EnqueueAsync(new SatelTask(TaskType.UpdateOutputs, new Dictionary<string, object> { { _ioIndex, incomingEvent.Payload } }));
        }
    }
}