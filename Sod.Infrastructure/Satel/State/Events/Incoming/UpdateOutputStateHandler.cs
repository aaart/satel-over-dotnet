using System;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;

namespace Sod.Infrastructure.Satel.State.Events.Incoming
{
    public class UpdateOutputStateHandler : IEventHandler
    {
        private readonly int _ioIndex;
        private readonly IManipulator _manipulator;

        public UpdateOutputStateHandler(int ioIndex, IManipulator manipulator)
        {
            _ioIndex = ioIndex;
            _manipulator = manipulator;
        }
        
        public async Task Handle(IncomingEvent incomingEvent)
        {
            var outputs = new bool[128];
            outputs[_ioIndex - 1] = true;
            await _manipulator.SwitchOutputs(outputs);
        }
    }
}