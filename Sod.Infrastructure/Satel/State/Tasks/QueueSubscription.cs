using System.Threading.Tasks;
using Sod.Infrastructure.Satel.State.Handlers;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.Satel.State.Tasks
{
    public class QueueSubscription : IQueueSubscription
    {
        private readonly IHandlerFactory _handlerFactory;

        public QueueSubscription(IHandlerFactory handlerFactory)
        {
            _handlerFactory = handlerFactory;
        }
        
        public async Task ReceiveTasks(ITaskQueue queue)
        {
            var (exists, task) = await queue.Dequeue();
            while (exists)
            {
                var handler = _handlerFactory.CreateHandler(task!.Type);
                await handler.Handle(task!.Parameters);
                (exists, task) = await queue.Dequeue();
            }
        }
    }
}