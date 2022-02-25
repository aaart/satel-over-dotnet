using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.State.Events.Outgoing;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Tasks.Handlers.Notifications
{
    public abstract class ChangeNotificationStateHandler : LoggingCapability, IStateHandler
    {
        private readonly IOutgoingEventPublisher _eventPublisher;

        protected ChangeNotificationStateHandler(IOutgoingEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }
        
        protected abstract OutgoingEventType OutgoingEventType { get; }
        
        public async Task<IEnumerable<SatelTask>> Handle(IReadOnlyDictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                var reference = Convert.ToInt32(parameter.Key);
                var value = Convert.ToBoolean(parameter.Value);
                await _eventPublisher.PublishAsync(new OutgoingEvent(OutgoingEventType, reference, value));
            }
            
            return Enumerable.Empty<SatelTask>();
        }
    }
}