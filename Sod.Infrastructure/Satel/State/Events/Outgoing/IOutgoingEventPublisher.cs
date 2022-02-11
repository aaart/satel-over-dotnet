using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.State.Events.Outgoing
{
    public interface IOutgoingEventPublisher
    {
        Task<IEnumerable<FailedOutgoingEvent>> PublishAsync(OutgoingEvent evnt);
    }
}