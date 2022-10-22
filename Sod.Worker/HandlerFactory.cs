using System;
using Autofac;
using Sod.Model.Tasks;
using Sod.Model.Tasks.Handlers;
using Sod.Model.Tasks.Handlers.Types;
using Sod.Model.Tasks.Types;

namespace Sod.Worker
{
    public class HandlerFactory : IHandlerFactory
    {
        private readonly IComponentContext _context;

        public HandlerFactory(IComponentContext context)
        {
            _context = context;
        }
        
        public ITaskHandler CreateHandler(SatelTask task)
        {
            switch (task)
            {
                case ActualStateOutputsUpdateTask:
                    return _context.Resolve<ActualStateOutputsUpdateTaskHandler>();
                case ActualStateBinaryIOReadTask:
                    return _context.Resolve<ActualStateBinaryIOReadTaskHandler>();
                case ActualStateChangedNotificationTask:
                    return _context.Resolve<ActualStateChangedNotificationTaskHandler>();
                case PersistedStateUpdateTask:
                    return _context.Resolve<PersistedStateUpdateTaskHandler>();
                case ActualStateAlarmIOPostReadTask:
                    return _context.Resolve<ActualStateAlarmIOPostReadTaskHandler>();
                case ActualStateBinaryIOPostReadTask:
                    return _context.Resolve<ActualStateBinaryIOPostReadTaskHandler>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(task), task.GetType(), "Not supported type.");
            }
        }
    }
}