using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Model.CommonTypes;

namespace Sod.Model.Events.Outgoing;

public interface IOutgoingEventPublisher
{
    Task<IEnumerable<FailedOutgoingEvent>> PublishAsync(OutgoingEvent evnt);
}