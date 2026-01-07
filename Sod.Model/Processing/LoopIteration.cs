using Sod.Infrastructure.Capabilities;
using Sod.Model.DataStructures;

namespace Sod.Model.Processing;

public class LoopIteration(
    IQueueProcessor processor,
    ITaskPlanner planner) : LoggingCapability, ILoopIteration
{
    public async Task IterationAsync(CancellationToken stoppingToken, ITaskQueue queue, int iteration)
    {
        await planner.Plan(queue, iteration);
        await processor.Process(queue);
    }
}