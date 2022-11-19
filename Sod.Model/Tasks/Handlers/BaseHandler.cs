using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Capabilities;

namespace Sod.Model.Tasks.Handlers;

public abstract class BaseHandler<T> : LoggingCapability, ITaskHandler
    where T : SatelTask
{
    public async Task<IEnumerable<SatelTask>> Handle(SatelTask data)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        return await Handle((T)data);
    }

    protected abstract Task<IEnumerable<SatelTask>> Handle(T data);
}