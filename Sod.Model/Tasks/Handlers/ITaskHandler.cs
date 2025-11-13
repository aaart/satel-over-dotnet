namespace Sod.Model.Tasks.Handlers;

public interface ITaskHandler
{
    Task<IEnumerable<SatelTask>> Handle(SatelTask data);
}