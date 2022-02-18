using Sod.Infrastructure.Enums;

namespace Sod.Infrastructure.State.Handlers
{
    public interface IHandlerFactory
    {
        IHandler CreateHandler(TaskType type);
    }
}