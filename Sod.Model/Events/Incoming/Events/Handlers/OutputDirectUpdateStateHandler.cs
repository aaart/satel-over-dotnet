using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;

namespace Sod.Model.Events.Incoming.Events.Handlers
{
    public class OutputDirectUpdateStateHandler : IEventHandler
    {
        private readonly int _ioIndex;
        private readonly IManipulator _manipulator;

        public OutputDirectUpdateStateHandler(int ioIndex, IManipulator manipulator)
        {
            _ioIndex = ioIndex;
            _manipulator = manipulator;
        }
        
        public async Task HandleAsync(IncomingEvent incomingEvent) // TODO: incomingEvent not used!
        {
            var outputs = new bool[128];
            outputs[_ioIndex - 1] = true;
            await _manipulator.SwitchOutputs(outputs);
        }
    }
}