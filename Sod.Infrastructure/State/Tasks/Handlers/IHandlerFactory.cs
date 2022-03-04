using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Tasks.Handlers
{
    public interface IHandlerFactory
    {
        ITaskHandler CreateHandler(SatelTask task);
    }
}