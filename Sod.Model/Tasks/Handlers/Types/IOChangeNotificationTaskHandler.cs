using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
            Logger.LogInformation($"Outgoing event type is {data.OutgoingEventType.ToString()}");
            foreach (var state in data.Notifications)
            {
                Logger.LogInformation($"index: {state.Index}, value: {state.Value}");
                await _eventPublisher.PublishAsync(new OutgoingEvent(data.OutgoingEventType, state.Index, state.Value));
            }
            
            return Enumerable.Empty<SatelTask>();
        }
    }
}