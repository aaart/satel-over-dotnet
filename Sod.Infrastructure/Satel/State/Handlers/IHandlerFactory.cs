using Sod.Infrastructure.Enums;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public interface IHandlerFactory
    {
        IHandler CreateHandler(TaskType type);
    }
}