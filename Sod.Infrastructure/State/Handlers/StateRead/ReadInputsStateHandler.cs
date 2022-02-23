using System.Threading.Tasks;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Handlers.StateRead
{
    public class ReadInputsStateHandler : ReadStateStateHandler
    {
        public ReadInputsStateHandler(
            IStore store, 
            IManipulator manipulator) 
            : base(store, manipulator)
        {
        }

        protected override string PersistedStateKey => Constants.Store.InputsState;
        protected override TaskType NotificationTaskType => TaskType.NotifyInputsChanged;
        protected override Task<(CommandStatus, bool[])> ManipulatorMethod(IManipulator manipulator) => manipulator.ReadInputs();
    }
}