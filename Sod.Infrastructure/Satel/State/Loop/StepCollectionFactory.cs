using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.State.Events;
using Sod.Infrastructure.Satel.State.Loop.StepType;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.Satel.State.Loop
{
    public class StepCollectionFactory : IStepCollectionFactory
    {
        private readonly int _milisecondsInterval;
        private readonly IStore _store;
        private readonly IManipulator _manipulator;
        private readonly IOutgoingChangeNotifier _outgoingChangeNotifier;

        public StepCollectionFactory(
            IStore store, 
            IManipulator manipulator,
            IOutgoingChangeNotifier outgoingChangeNotifier,
            int milisecondsInterval = 10)
        {
            _store = store;
            _manipulator = manipulator;
            _outgoingChangeNotifier = outgoingChangeNotifier;
            _milisecondsInterval = milisecondsInterval;
        }


        public IStepCollection BuildStepCollection()
        {
            return new StepCollection(_milisecondsInterval)
            {
                // update removed temporary
                //new UpdateOutputs(_store, _manipulator, _eventPublisher),
                new ReadOutputs(_store, _manipulator, _outgoingChangeNotifier),
                new ReadInputs(_store, _manipulator, _outgoingChangeNotifier)
            };
        }

        public async Task BuildAndExecuteStepCollection() => await BuildStepCollection().ExecuteAsync();
    }
}