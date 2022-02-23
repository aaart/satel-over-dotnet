using System.Threading.Tasks;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Handlers.StateRead
{
    public class ReadOutputsStateHandler : ReadStateStateHandler
    {
        public ReadOutputsStateHandler(
            IStore store,
            IManipulator manipulator) 
            : base(store, manipulator)
        {
        }

        protected override string PersistedStateKey => Constants.Store.OutputsState;
        protected override TaskType NotificationTaskType => TaskType.NotifyOutputsChanged;
        protected override Task<(CommandStatus, bool[])> ManipulatorMethod(IManipulator manipulator) => manipulator.ReadOutputs();
    }
}