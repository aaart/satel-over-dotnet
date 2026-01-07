namespace Sod.Model.DataStructures;

public class GlobalState
{
    public string Timestamp { get; set; } = string.Empty;
    public bool[] Inputs { get; set; } = Array.Empty<bool>();
    public bool[] Outputs { get; set; } = Array.Empty<bool>();
    public bool[] ArmedPartitions { get; set; } = Array.Empty<bool>();
    public bool[] TriggeredPartitions { get; set; } = Array.Empty<bool>();
    public bool[] SuppressedPartitions { get; set; } = Array.Empty<bool>();
}
