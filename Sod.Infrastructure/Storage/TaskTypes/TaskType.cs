namespace Sod.Infrastructure.Storage.TaskTypes
{
    public enum TaskType
    {
        Invalid,
        UpdateOutputs,
        ReadIOState,
        UpdateStorage,
        NotifyIOChanged
    }
}