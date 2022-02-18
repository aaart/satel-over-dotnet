using System.Threading.Tasks;

namespace Sod.Infrastructure.State.Events.Incoming
{
    public interface IBroker
    {
        Task Process(IncomingEvent incomingEvent);
    }
}