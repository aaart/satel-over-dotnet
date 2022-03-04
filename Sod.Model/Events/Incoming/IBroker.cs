using System.Threading.Tasks;

namespace Sod.Model.Events.Incoming
{
    public interface IBroker
    {
        Task Process(IncomingEvent incomingEvent);
    }
}