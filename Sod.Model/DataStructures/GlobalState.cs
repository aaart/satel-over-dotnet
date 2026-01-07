namespace Sod.Model.DataStructures;

public record GlobalState
{
    public string Timestamp { get; init; } = string.Empty;
    public bool[] Inputs { get; init; } = Array.Empty<bool>();
    public bool[] Outputs { get; init; } = Array.Empty<bool>();
    public bool[] ArmedPartitions { get; init; } = Array.Empty<bool>();
    public bool[] TriggeredPartitions { get; init; } = Array.Empty<bool>();
    public bool[] SuppressedPartitions { get; init; } = Array.Empty<bool>();
}
