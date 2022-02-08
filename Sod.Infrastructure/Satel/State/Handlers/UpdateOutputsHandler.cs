using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.Communication;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public class UpdateOutputsHandler : LoggingCapability, IHandler
    {
        private readonly IManipulator _manipulator;

        public UpdateOutputsHandler(IManipulator manipulator)
        {
            Logger.LogDebug($"{nameof(UpdateOutputsHandler)} is executing.");
            _manipulator = manipulator;
        }


        public Task Handle(IReadOnlyDictionary<string, object> parameters)
        {
            return Task.CompletedTask;
        }
    }
}