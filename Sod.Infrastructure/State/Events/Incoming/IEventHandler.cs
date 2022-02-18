using System.Threading.Tasks;

namespace Sod.Infrastructure.State.Events.Incoming
{
    public interface IEventHandler
    {
        Task HandleAsync(IncomingEvent incomingEvent);
    }
}