using Sod.Model.DataStructures;

namespace Sod.Model.Processing;

public interface IQueueProcessor
{
    Task Process(ITaskQueue queue);
}