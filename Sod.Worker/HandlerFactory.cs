using System;
using System.Collections.Generic;
using Autofac;
using Sod.Model.Tasks;
using Sod.Model.Tasks.Handlers;
using Sod.Model.Tasks.Handlers.Types;
using Sod.Model.Tasks.Types;

namespace Sod.Worker;

public class HandlerFactory : IHandlerFactory
{
    private readonly IComponentContext _context;
    private readonly Dictionary<Type, Type> _handlerMappings;

    public HandlerFactory(IComponentContext context)
    {
        _context = context;
        _handlerMappings = new Dictionary<Type, Type>
        {
            { typeof(ActualStateBinaryIOUpdateTask), typeof(ActualStateBinaryIOUpdateTaskHandler) },
            { typeof(ActualStateBinaryIOReadTask), typeof(ActualStateBinaryIOReadTaskHandler) },
            { typeof(ActualStateChangedNotificationTask), typeof(ActualStateChangedNotificationTaskHandler) },
            { typeof(PersistedStateUpdateTask), typeof(PersistedStateUpdateTaskHandler) },
            { typeof(ActualStateAlarmIOPostReadTask), typeof(ActualStateAlarmIOPostReadTaskHandler) },
            { typeof(ActualStateBinaryIOPostReadTask), typeof(ActualStateBinaryIOPostReadTaskHandler) }
        };
    }

    public ITaskHandler CreateHandler(SatelTask task)
    {
        var taskType = task.GetType();
        
        if (!_handlerMappings.TryGetValue(taskType, out var handlerType))
        {
            throw new ArgumentOutOfRangeException(nameof(task), taskType, "Not supported type.");
        }

        return (ITaskHandler)_context.Resolve(handlerType);
    }
}