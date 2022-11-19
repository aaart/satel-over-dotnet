using Sod.Infrastructure.Capabilities;
using Sod.Model.DataStructures;
using Sod.Model.Processing.Exceptions;

namespace Sod.Model.Processing;

public class Loop : LoggingCapability, ILoop
{
    private readonly ILoopIteration _loopIteration;
    private readonly ITaskQueue _queue;
    private readonly LoopOptions _options;
    private readonly ILoopIterationExceptionHandlingPolicy _loopIterationExceptionHandlingPolicy;

    public Loop(
        ITaskQueue queue,
        LoopOptions options,
        ILoopIteration loopIteration,
        ILoopIterationExceptionHandlingPolicy loopIterationExceptionHandlingPolicy)
    {
        _queue = queue;
        _options = options;
        _loopIteration = loopIteration;
        _loopIterationExceptionHandlingPolicy = loopIterationExceptionHandlingPolicy;
    }

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var iteration = 0;
        do
        {
            try
            {
                await _loopIteration.IterationAsync(stoppingToken, _queue, iteration);
                iteration = iteration < _options.IterationCount ? iteration + 1 : 0;

                // just a safe-switch. if everything fails, and the app goes to abnormal state report it. It is expected that Iteration handling policy will report it.
                if (iteration > _options.IterationCount) throw new SodCriticalException(SodCriticalExceptionReason.IterationExceededExpectedLimit);

                await Task.Delay(_options.Interval, stoppingToken);
            }
            catch (Exception e)
            {
                await Task.Delay(_options.OnErrorDelayMiliseconds);
                iteration = await _loopIterationExceptionHandlingPolicy.HandleExceptionAsync(e, _queue);
            }
        } while (!stoppingToken.IsCancellationRequested);
    }
}