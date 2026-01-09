using System.Collections.Generic;
using Sod.Infrastructure.Satel;
using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks;

namespace Sod.Model.Events.Incoming.Factories;

public class BinaryOutputTaskFactory : ISatelTaskFactory
{
    public IncomingEventType Type => IncomingEventType.BinaryOutput;

    public BaseSatelTask CreateTask(string payload, int ioIndex, bool notify)
    {
        return new ActualStateBinaryIOUpdateTask(
            new List<BinaryIOState> { new() { Index = ioIndex, Value = OnOffParse.ToBoolean(payload) } },
            IOBinaryUpdateType.Outputs,
            notify,
            OutgoingEventType.OutputsStateChanged,
            128);
    }
}
