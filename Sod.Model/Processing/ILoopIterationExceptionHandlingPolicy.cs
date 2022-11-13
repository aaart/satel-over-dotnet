using Sod.Model.DataStructures;

namespace Sod.Model.Processing;

public interface ILoopIterationExceptionHandlingPolicy
{
    Task<int> HandleExceptionAsync(Exception exception, ITaskQueue queue);
}