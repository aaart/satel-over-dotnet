using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Model.DataStructures;

namespace Sod.Model.Processing;

public class LoopIterationExceptionHandlingPolicy : LoggingCapability, ILoopIterationExceptionHandlingPolicy
{
    public async Task<int> HandleExceptionAsync(Exception exception, ITaskQueue queue)
    {
        await queue.Clear();
        Logger.LogError(exception, exception.Message);
        return 0;
    }
}