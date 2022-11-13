using System;
using System.Threading.Tasks;
using Sod.Model.DataStructures;
using Sod.Model.Processing;

namespace Sod.Tests.Processing.ProcessingHelpers;

public class TestLoopIterationExceptionHandlingPolicy : ILoopIterationExceptionHandlingPolicy
{
    private readonly Action _action;

    public TestLoopIterationExceptionHandlingPolicy(Action action)
    {
        _action = action;
    }
    
    public Task<int> HandleExceptionAsync(Exception exception, ITaskQueue queue)
    {
        _action();
        return Task.FromResult(0);
    }
}