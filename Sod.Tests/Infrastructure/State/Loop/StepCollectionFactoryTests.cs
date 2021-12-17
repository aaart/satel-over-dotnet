using System.Linq;
using FluentAssertions;
using Moq;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.State.Events;
using Sod.Infrastructure.Satel.State.Loop;
using Sod.Infrastructure.Satel.State.Loop.StepType;
using Sod.Infrastructure.Store;
using Xunit;

namespace Sod.Tests.Infrastructure.State.Loop
{
    public class StepCollectionFactoryTests
    {
        
        [Fact]
        public void GivenBuilder_WhenStoreDoesNotContainStepIndicator_ExpectDefaultCollectionOrder()
        {
            var store = new Mock<IStore>();
            var manipulator = new Mock<IManipulator>();
            var eventPublisher = new Mock<IOutgoingChangeNotifier>();
            
            var builder = new StepCollectionFactory(store.Object, manipulator.Object, eventPublisher.Object);

            var steps = builder.BuildStepCollection();
            var stepTypes = steps.Select(x => x.GetType());
            stepTypes
                .Should().HaveElementAt(0, typeof(ReadOutputs))
                .And.HaveElementAt(1, typeof(ReadInputs));
        }
    }
}