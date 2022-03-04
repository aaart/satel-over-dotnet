using System.Threading.Tasks;
using Sod.Model.DataStructures;

namespace Sod.Model.Events.Incoming
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
        
        public Task HandleAsync(IncomingEvent incomingEvent)
        {
            return Task.CompletedTask;
        }
    }
}