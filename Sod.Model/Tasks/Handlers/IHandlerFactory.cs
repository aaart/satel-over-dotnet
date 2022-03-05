namespace Sod.Model.Tasks.Handlers
{
    public interface IHandlerFactory
    {
        ITaskHandler CreateHandler(SatelTask task);
    }
}