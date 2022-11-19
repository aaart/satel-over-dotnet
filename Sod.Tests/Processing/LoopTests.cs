using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Sod.Model.DataStructures;
using Sod.Model.Processing;
using Sod.Tests.Processing.ProcessingHelpers;
using Xunit;

namespace Sod.Tests.Processing;

public class LoopTests
{
    [Fact]
    public async Task GivenLoop_WhenExceptionIsThrown_ExpectHandlerInvoked()
    {
        var handled = false;
        var loop = new Loop(
            new Mock<ITaskQueue>().Object,
            new LoopOptions { Interval = 1, IterationCount = 1, OnErrorDelayMiliseconds = 100 },
            new TestLoopIteration(new Mock<IQueueProcessor>().Object, new Mock<ITaskPlanner>().Object, () => throw new Exception()),
            new TestLoopIterationExceptionHandlingPolicy(() => handled = true));

        var source = new CancellationTokenSource();
        source.Cancel();
        await loop.ExecuteAsync(source.Token);
        Assert.True(handled);
    }
}