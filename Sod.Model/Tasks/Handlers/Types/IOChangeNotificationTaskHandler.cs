using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sod.Model.CommonTypes;
using Sod.Model.Events.Outgoing;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types
{
    public class IOChangeNotificationTaskHandler : BaseHandler<IOChangeNotificationTask>
    {
        private readonly IOutgoingEventPublisher _eventPublisher;

        public IOChangeNotificationTaskHandler(IOutgoingEventPublisher eventPublisher)
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