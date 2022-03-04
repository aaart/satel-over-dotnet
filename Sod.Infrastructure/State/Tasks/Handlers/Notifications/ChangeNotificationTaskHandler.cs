using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.State.Events.Outgoing;
using Sod.Infrastructure.Storage;
using Sod.Infrastructure.Storage.TaskTypes.Notifications;

namespace Sod.Infrastructure.State.Tasks.Handlers.Notifications
{
    public class ChangeNotificationTaskHandler : BaseHandler<IOChangeNotificationTask>
    {
        private readonly IOutgoingEventPublisher _eventPublisher;

        public ChangeNotificationTaskHandler(IOutgoingEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        protected override async Task<IEnumerable<SatelTask>> Handle(IOChangeNotificationTask data)
        {
            foreach (var state in data.Notifications)
            {
                await _eventPublisher.PublishAsync(new OutgoingEvent(data.OutgoingEventType, state.Index, state.Value));
            }
            
            return Enumerable.Empty<SatelTask>();
        }
    }
}