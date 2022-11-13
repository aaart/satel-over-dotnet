using Sod.Infrastructure.Capabilities;
using Sod.Model.DataStructures;

namespace Sod.Model.Processing;

public class LoopIteration : LoggingCapability, ILoopIteration
{
    private readonly IQueueProcessor _processor;
    private readonly ITaskPlanner _planner;

    public LoopIteration(
        IQueueProcessor processor,
        ITaskPlanner planner)
    {
        _processor = processor;
        _planner = planner;
    }

    public async Task IterationAsync(CancellationToken stoppingToken, ITaskQueue queue, int iteration)
    {
        await _planner.Plan(queue, iteration);
        await _processor.Process(queue);
    }
}