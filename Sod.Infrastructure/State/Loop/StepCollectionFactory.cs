using Sod.Infrastructure.Satel;
using Sod.Infrastructure.State.Events;
using Sod.Infrastructure.State.Loop.StepType;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.State.Loop
{
    public class StepCollectionFactory : IStepCollectionFactory
    {
        private readonly int _milisecondsInterval;
        private readonly IStore _store;
        private readonly IManipulator _manipulator;
        private readonly IEventPublisher _eventPublisher;

        public StepCollectionFactory(
            IStore store, 
            IManipulator manipulator,
            IEventPublisher eventPublisher,
            int milisecondsInterval = 10)
        {
            _store = store;
            _manipulator = manipulator;
            _eventPublisher = eventPublisher;
            _milisecondsInterval = milisecondsInterval;
        }


        public IStepCollection BuildStepCollection()
        {
            return new StepCollection(_milisecondsInterval)
            {
                new UpdateOutputs(_store, _manipulator, _eventPublisher),
                new ReadOutputs(_store, _manipulator, _eventPublisher),
                new ReadInputs(_store, _manipulator, _eventPublisher)
            };
        }
    }
}