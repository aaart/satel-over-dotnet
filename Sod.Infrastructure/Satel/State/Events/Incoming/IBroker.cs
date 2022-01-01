using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.State.Events.Incoming
{
    public interface IBroker
    {
        Task Process(IncomingEvent incomingEvent);
    }
}