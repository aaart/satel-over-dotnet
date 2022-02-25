using Sod.Infrastructure.Enums;

namespace Sod.Infrastructure.State.Tasks.Handlers
{
    public interface IHandlerFactory
    {
        IStateHandler CreateHandler(TaskType type);
    }
}