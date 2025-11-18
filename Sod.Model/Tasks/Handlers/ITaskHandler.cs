namespace Sod.Model.Tasks.Handlers;

public interface ITaskHandler
{
    Task<IEnumerable<BaseSatelTask>> Handle(BaseSatelTask data);
}