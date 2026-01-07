using Newtonsoft.Json;
using Sod.Model.DataStructures;
using Sod.Model.Events.Outgoing;

namespace Sod.Model.Tasks.Handlers.Impl;

public class ActualStateGlobalBroadcastTaskHandler(IStore store, IOutgoingEventPublisher eventPublisher)
    : BaseHandler<ActualStateGlobalBroadcastTask>
{
    protected override async Task<IEnumerable<BaseSatelTask>> Handle(ActualStateGlobalBroadcastTask data)
    {
        var inputs = await store.GetAsync<bool[]>(Constants.Store.InputsState);
        var outputs = await store.GetAsync<bool[]>(Constants.Store.OutputsState);
        var armedPartitions = await store.GetAsync<bool[]>(Constants.Store.ArmedPartitions);
        var triggeredPartitions = await store.GetAsync<bool[]>(Constants.Store.TriggeredPartitions);
        var suppressedPartitions = await store.GetAsync<bool[]>(Constants.Store.SuppressedPartitions);

        var globalState = new GlobalState
        {
            Timestamp = data.Timestamp,
            Inputs = inputs,
            Outputs = outputs,
            ArmedPartitions = armedPartitions,
            TriggeredPartitions = triggeredPartitions,
            SuppressedPartitions = suppressedPartitions
        };

        var json = JsonConvert.SerializeObject(globalState);
        await eventPublisher.PublishAsync(new OutgoingEvent(OutgoingEventType.GlobalBroadcast, 0, json));

        return Enumerable.Empty<BaseSatelTask>();
    }
}
