using System;
using Microsoft.Extensions.DependencyInjection;
using Sod.Model.Tasks.Handlers.Types;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers
{
    public class HandlerFactory : IHandlerFactory
    {
        private readonly IServiceProvider _provider;

        public HandlerFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public ITaskHandler CreateHandler(SatelTask task)
        {
            switch (task)
            {
                case OutputsUpdateTask:
                    return _provider.GetRequiredService<OutputsUpdateTaskHandler>();
                case ReadStateTask:
                    return _provider.GetRequiredService<ReadStateTaskHandler>();
                case StorageUpdateTask:
                    return _provider.GetRequiredService<StorageUpdateTaskHandler>();
                case IOChangeNotificationTask:
                    return _provider.GetRequiredService<IOChangeNotificationTaskHandler>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(task), task.GetType(), "Not supported type.");
            }
        }
    }
}