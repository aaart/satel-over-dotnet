using System;
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


        public async Task Handle(IReadOnlyDictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                var outputs = new bool[128];
                outputs[Convert.ToInt32(parameter.Key) - 1] = true;
                await _manipulator.SwitchOutputs(outputs);
            }
        }
    }
}