using System.Threading.Tasks;

namespace Sod.Model.Events.Incoming
{
    public interface IEventHandler
    {
        Task HandleAsync(IncomingEvent incomingEvent);
    }
}