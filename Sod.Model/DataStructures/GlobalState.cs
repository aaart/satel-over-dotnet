using Newtonsoft.Json;

namespace Sod.Model.DataStructures;

public class GlobalState
{
    [JsonProperty("timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    [JsonProperty("inputs")]
    public bool[] Inputs { get; set; } = Array.Empty<bool>();

    [JsonProperty("outputs")]
    public bool[] Outputs { get; set; } = Array.Empty<bool>();

    [JsonProperty("armedPartitions")]
    public bool[] ArmedPartitions { get; set; } = Array.Empty<bool>();

    [JsonProperty("triggeredPartitions")]
    public bool[] TriggeredPartitions { get; set; } = Array.Empty<bool>();

    [JsonProperty("suppressedPartitions")]
    public bool[] SuppressedPartitions { get; set; } = Array.Empty<bool>();
}
