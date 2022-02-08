using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public class ReadOutputsHandler : ReadStateHandler
    {
        public ReadOutputsHandler(
            IStore store, 
            ITaskQueue queue, 
            IManipulator manipulator) 
            : base(store, queue, manipulator)
        {
        }

        protected override string PersistedStateKey => Constants.Store.OutputsState;
        protected override Task<(CommandStatus, bool[])> ManipulatorMethod(IManipulator manipulator) => manipulator.ReadOutputs();
    }
}