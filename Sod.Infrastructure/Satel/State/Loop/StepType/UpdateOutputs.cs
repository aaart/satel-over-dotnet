using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.State.Events;
using Sod.Infrastructure.Satel.State.Events.Outgoing;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.Satel.State.Loop.StepType
{
    public class UpdateOutputs : BaseStep
    {
        public UpdateOutputs(IStore store, IManipulator manipulator, IOutgoingEventPublisher outgoingEventPublisher) 
            : base(store, manipulator, outgoingEventPublisher)
        {
        }

        protected override Task ExecuteInternalAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}