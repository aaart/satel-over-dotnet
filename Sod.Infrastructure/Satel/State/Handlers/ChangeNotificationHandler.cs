using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.State.Events.Outgoing;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public abstract class ChangeNotificationHandler : LoggingCapability, IHandler
    {
        private readonly IOutgoingEventPublisher _eventPublisher;

        protected ChangeNotificationHandler(IOutgoingEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }
        
        protected abstract OutgoingEventType OutgoingEventType { get; }
        
        public async Task Handle(IReadOnlyDictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                var reference = Convert.ToInt32(parameter.Key);
                var value = Convert.ToBoolean(parameter.Value);
                await _eventPublisher.PublishAsync(new OutgoingEvent(OutgoingEventType, reference, value));
            }
        }
    }
}