using Sod.Model.DataStructures;

namespace Sod.Model.Processing;

public interface ILoopIteration
{
    Task IterationAsync(CancellationToken stoppingToken, ITaskQueue queue, int iteration);
}