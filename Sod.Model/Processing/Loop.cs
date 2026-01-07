using Sod.Infrastructure.Capabilities;
using Sod.Model.DataStructures;
using Sod.Model.Processing.Exceptions;

namespace Sod.Model.Processing;

public class Loop(
    ITaskQueue queue,
    LoopOptions options,
    ILoopIteration loopIteration,
    ILoopIterationExceptionHandlingPolicy loopIterationExceptionHandlingPolicy)
    : LoggingCapability, ILoop
{
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var iteration = 0;
        do
        {
            try
            {
                await loopIteration.IterationAsync(stoppingToken, queue, iteration);
                iteration = iteration < options.IterationCount ? iteration + 1 : 0;

                // just a safe-switch. if everything fails, and the app goes to abnormal state report it. It is expected that Iteration handling policy will report it.
                if (iteration > options.IterationCount) throw new SodCriticalException(SodCriticalExceptionReason.IterationExceededExpectedLimit);

                await Task.Delay(options.Interval, stoppingToken);
            }
            catch (Exception e)
            {
                await Task.Delay(options.OnErrorDelayMiliseconds);
                iteration = await loopIterationExceptionHandlingPolicy.HandleExceptionAsync(e, queue);
            }
        } while (!stoppingToken.IsCancellationRequested);
    }
}