using System.Threading.Tasks;
using Sod.Model.Events.Incoming.Events;

namespace Sod.Model.Events.Incoming
{
    public interface IBroker
    {
        Task Process(IncomingEvent incomingEvent);
    }
}