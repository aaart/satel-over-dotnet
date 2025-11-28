using Sod.Model.DataStructures;
using Sod.Model.Tasks.Handlers;

namespace Sod.Model.Processing;

public class QueueProcessor(IHandlerFactory handlerFactory) : IQueueProcessor
{
    public async Task Process(ITaskQueue queue)
    {
        var (exists, task) = await queue.DequeueAsync();
        while (exists)
        {
            var handler = handlerFactory.CreateHandler(task!);
            var tasks = await handler.Handle(task!);
            foreach (var newTask in tasks) await queue.EnqueueAsync(newTask);

            (exists, task) = await queue.DequeueAsync();
        }
    }
}