namespace Sod.Model.Tasks.Types;

public class PersistedStateUpdateTask(string storageKey, bool[] values) : SatelTask
{
    public string StorageKey { get; } = storageKey;
    public bool[] Values { get; } = values;
}