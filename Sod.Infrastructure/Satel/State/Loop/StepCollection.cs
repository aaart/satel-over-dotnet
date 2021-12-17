using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.State.Loop
{
    public class StepCollection : List<IStep>, IStepCollection
    {
        private readonly int _milisecondsInterval;

        public StepCollection(int milisecondsInterval)
        {
            _milisecondsInterval = milisecondsInterval;
        }
        
        public async Task ExecuteAsync()
        {
            foreach (var step in this)
            {
                await step.ExecuteAsync();
                await Task.Delay(_milisecondsInterval);
            }
        }
    }
}