using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public class ReadInputsHandler : ReadStateHandler
    {
        public ReadInputsHandler(
            IStore store, 
            ITaskQueue queue, 
            IManipulator manipulator) 
            : base(store, queue, manipulator)
        {
        }

        protected override string PersistedStateKey => Constants.Store.InputsState;
        protected override TaskType NotificationTaskType => TaskType.NotifyInputsChanged;
        protected override Task<(CommandStatus, bool[])> ManipulatorMethod(IManipulator manipulator) => manipulator.ReadInputs();
    }
}