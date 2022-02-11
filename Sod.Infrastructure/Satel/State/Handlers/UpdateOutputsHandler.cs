using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public class UpdateOutputsHandler : LoggingCapability, IHandler
    {
        private readonly IStore _store;
        private readonly IManipulator _manipulator;

        public UpdateOutputsHandler(IStore store, IManipulator manipulator)
        {
            Logger.LogDebug($"{nameof(UpdateOutputsHandler)} is executing.");
            _store = store;
            _manipulator = manipulator;
        }


        public async Task Handle(IReadOnlyDictionary<string, object> parameters)
        {
            var persistedState = await _store.GetAsync<bool[]>(Constants.Store.OutputsState);
            var outputs = new bool[128];
            foreach (var parameter in parameters)
            {
                var index = Convert.ToInt32(parameter.Key) - 1;
                outputs[index] = persistedState[index] != OnOffParse.ToBoolean(Convert.ToString(parameter.Value)!);
            }
            await _manipulator.SwitchOutputs(outputs);
        }
    }
}