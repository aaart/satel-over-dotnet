using Sod.Infrastructure.Satel;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.State.Loop.StepType
{
    public class ReadInputs : ReadOutputs
    {
        public ReadInputs(IStore store, IManipulator manipulator) 
            : base(store, manipulator)
        {
        }
    }
}