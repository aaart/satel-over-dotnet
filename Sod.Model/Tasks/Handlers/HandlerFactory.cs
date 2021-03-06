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
                case ActualStateOutputsUpdateTask:
                    return _provider.GetRequiredService<ActualStateOutputsUpdateTaskHandler>();
                case ActualStateReadTask:
                    return _provider.GetRequiredService<ActualStateReadTaskHandler>();
                case ActualStateChangedNotificationTask:
                    return _provider.GetRequiredService<ActualStateChangedNotificationTaskHandler>();
                case PersistedStateUpdateTask:
                    return _provider.GetRequiredService<PersistedStateUpdateTaskHandler>();
                case PersistedStateAlarmStateReadTask:
                    return _provider.GetRequiredService<PersistedStateAlarmStateReadTaskHandler>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(task), task.GetType(), "Not supported type.");
            }
        }
    }
}