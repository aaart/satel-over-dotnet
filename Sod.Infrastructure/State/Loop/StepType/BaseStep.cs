using System.Threading.Tasks;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.State.Loop.StepType
{
    public abstract class BaseStep : IStep
    {
        protected IStore Store { get; }
        protected IManipulator Manipulator { get; }

        protected BaseStep(IStore store, IManipulator manipulator)
        {
            Store = store;
            Manipulator = manipulator;
        }

        public abstract Task ExecuteAsync();
    }
}