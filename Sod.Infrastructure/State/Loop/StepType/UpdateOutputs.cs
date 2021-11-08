using System.Threading.Tasks;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.State.Loop.StepType
{
    public class UpdateOutputs : BaseStep
    {
        public UpdateOutputs(IStore store, IManipulator manipulator) 
            : base(store, manipulator)
        {
        }

        public override Task ExecuteAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}