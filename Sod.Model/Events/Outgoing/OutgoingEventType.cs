namespace Sod.Model.Events.Outgoing;

public enum OutgoingEventType
{
    InputsStateChanged,
    OutputsStateChanged,
    ArmedPartitionsStateChanged,
    SuppressedPartitionsStateChanged,
    PartitionTriggered
}