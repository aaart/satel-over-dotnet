using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public class SwitchOutputsHandler : LoggingCapability, IHandler
    {
        private readonly IManipulator _manipulator;

        public SwitchOutputsHandler(IManipulator manipulator)
        {
            Logger.LogDebug($"{nameof(SwitchOutputsHandler)} is executing.");
            _manipulator = manipulator;
        }


        public async Task Handle(IReadOnlyDictionary<string, object> parameters)
        {
            var outputs = new bool[128];
            foreach (var parameter in parameters)
            {
                var index = Convert.ToInt32(parameter.Key) - 1;
                outputs[index] = true;
            }
            await _manipulator.SwitchOutputs(outputs);
        }
    }
}