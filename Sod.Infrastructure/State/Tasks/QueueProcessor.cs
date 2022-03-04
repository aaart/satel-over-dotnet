using System;
using System.Threading.Tasks;
using Sod.Infrastructure.State.Tasks.Handlers;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Tasks
{
    public class QueueProcessor : IQueueProcessor
    {
        private readonly IHandlerFactory _handlerFactory;

        public QueueProcessor(IHandlerFactory handlerFactory)
        {
            _handlerFactory = handlerFactory;
        }

        public async Task Process(ITaskQueue queue)
        {
            var (exists, task) = await queue.DequeueAsync();
            while (exists)
            {
                var handler = _handlerFactory.CreateHandler(task!);
                var tasks = await handler.Handle(task!);
                foreach (var newTask in tasks)
                {
                    await queue.EnqueueAsync(newTask);
                }

                (exists, task) = await queue.DequeueAsync();
            }
        }
    }
}