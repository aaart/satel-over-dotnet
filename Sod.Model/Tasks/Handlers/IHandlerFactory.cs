namespace Sod.Model.Tasks.Handlers;

public interface IHandlerFactory
{
    ITaskHandler CreateHandler(BaseSatelTask task);
}