using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.State.Events;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.Satel.State.Loop.StepType
{
    public class UpdateOutputs : BaseStep
    {
        public UpdateOutputs(IStore store, IManipulator manipulator, IOutgoingChangeNotifier outgoingChangeNotifier) 
            : base(store, manipulator, outgoingChangeNotifier)
        {
        }

        protected override Task ExecuteInternalAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}