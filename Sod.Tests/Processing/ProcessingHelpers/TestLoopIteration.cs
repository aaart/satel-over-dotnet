using System;
using Sod.Model.Processing;

namespace Sod.Tests.Processing.ProcessingHelpers;

public class TestLoopIteration : LoopIteration
{
    private readonly Action _action;

    public TestLoopIteration(
        IQueueProcessor processor, 
        ITaskPlanner planner,
        Action action) 
        : base(processor, planner)
    {
        _action = action;
    }
}