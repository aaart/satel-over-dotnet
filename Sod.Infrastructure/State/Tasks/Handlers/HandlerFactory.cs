using System;
using Microsoft.Extensions.DependencyInjection;
using Sod.Infrastructure.State.Tasks.Handlers.IOStateRead;
using Sod.Infrastructure.State.Tasks.Handlers.Notifications;
using Sod.Infrastructure.State.Tasks.Handlers.OutputsUpdate;
using Sod.Infrastructure.State.Tasks.Handlers.StorageUpdate;
using Sod.Infrastructure.Storage;
using Sod.Infrastructure.Storage.TaskTypes;
using Sod.Infrastructure.Storage.TaskTypes.IOStateRead;
using Sod.Infrastructure.Storage.TaskTypes.Notifications;
using Sod.Infrastructure.Storage.TaskTypes.OutputsUpdate;
using Sod.Infrastructure.Storage.TaskTypes.StorageUpdate;

namespace Sod.Infrastructure.State.Tasks.Handlers
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
                    return _provider.GetRequiredService<UpdateOutputsTaskHandler>();
                case ReadStateTask:
                    return _provider.GetRequiredService<ReadIOStateTaskHandler>();
                case StorageUpdateTask:
                    return _provider.GetRequiredService<UpdateStorageTaskHandler>();
                case IOChangeNotificationTask:
                    return _provider.GetRequiredService<ChangeNotificationTaskHandler>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(task), task.GetType(), "Not supported type.");
            }
        }
    }
}