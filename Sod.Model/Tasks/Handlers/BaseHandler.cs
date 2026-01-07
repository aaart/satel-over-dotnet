using Sod.Infrastructure.Capabilities;

namespace Sod.Model.Tasks.Handlers;

public abstract class BaseHandler<T> : LoggingCapability, ITaskHandler
    where T : BaseSatelTask
{
    public async Task<IEnumerable<BaseSatelTask>> Handle(BaseSatelTask data)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        return await Handle((T)data);
    }

    protected abstract Task<IEnumerable<BaseSatelTask>> Handle(T data);
}