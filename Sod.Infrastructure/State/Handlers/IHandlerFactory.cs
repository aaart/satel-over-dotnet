using Sod.Infrastructure.Enums;

namespace Sod.Infrastructure.State.Handlers
{
    public interface IHandlerFactory
    {
        IStateHandler CreateHandler(TaskType type);
    }
}