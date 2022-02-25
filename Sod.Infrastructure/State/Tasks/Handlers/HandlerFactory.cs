using System;
using Microsoft.Extensions.DependencyInjection;
using Sod.Infrastructure.Enums;
using Sod.Infrastructure.State.Tasks.Handlers.Notifications;
using Sod.Infrastructure.State.Tasks.Handlers.OutputsUpdate;
using Sod.Infrastructure.State.Tasks.Handlers.StateRead;
using Sod.Infrastructure.State.Tasks.Handlers.StorageUpdate;

namespace Sod.Infrastructure.State.Tasks.Handlers
{
    public class HandlerFactory : IHandlerFactory
    {
        private readonly IServiceProvider _provider;

        public HandlerFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public IStateHandler CreateHandler(TaskType type)
        {
            switch (type)
            {
                case TaskType.UpdateOutputs:
                    return _provider.GetRequiredService<UpdateOutputsStateHandler>();
                case TaskType.ReadInputs:
                    return _provider.GetRequiredService<ReadInputsStateHandler>();
                case TaskType.ReadOutputs:
                    return _provider.GetRequiredService<ReadOutputsStateHandler>();
                case TaskType.UpdateStorage:
                    return _provider.GetRequiredService<UpdateStorageStateHandler>();
                case TaskType.NotifyInputsChanged:
                    return _provider.GetRequiredService<InputsChangeNotificationStateHandler>();
                case TaskType.NotifyOutputsChanged:
                    return _provider.GetRequiredService<OutputsChangeNotificationStateHandler>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}