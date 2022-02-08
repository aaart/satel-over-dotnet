using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sod.Infrastructure.Enums;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public class HandlerFactory : IHandlerFactory
    {
        private readonly IServiceProvider _provider;

        public HandlerFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public IHandler CreateHandler(TaskType type)
        {
            switch (type)
            {
                case TaskType.UpdateOutputs:
                    return _provider.GetRequiredService<UpdateOutputsHandler>();
                case TaskType.ReadInputs:
                    return _provider.GetRequiredService<ReadInputsHandler>();
                case TaskType.ReadOutputs:
                    return _provider.GetRequiredService<ReadOutputsHandler>();
                case TaskType.UpdateStorage:
                    return _provider.GetRequiredService<UpdateStorageHandler>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}