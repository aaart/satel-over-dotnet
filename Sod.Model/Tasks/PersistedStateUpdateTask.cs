namespace Sod.Model.Tasks;

public class PersistedStateUpdateTask(string storageKey, bool[] values) : BaseSatelTask
{
    public string StorageKey { get; } = storageKey;
    public bool[] Values { get; } = values;
}