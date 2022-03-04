using System.Threading.Tasks;
using Sod.Model.Events.Incoming.Events;

namespace Sod.Model.Events.Incoming
{
    public interface IEventHandler
    {
        Task HandleAsync(IncomingEvent incomingEvent);
    }
}