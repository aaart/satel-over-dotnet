using System;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.State.Events;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.Satel.State.Loop.StepType
{
    public abstract class BaseStep : IStep
    {
        protected IStore Store { get; }
        protected IManipulator Manipulator { get; }
        protected IOutgoingChangeNotifier OutgoingChangeNotifier { get; }

        protected BaseStep(
            IStore store, 
            IManipulator manipulator,
            IOutgoingChangeNotifier outgoingChangeNotifier)
        {
            Store = store;
            Manipulator = manipulator;
            OutgoingChangeNotifier = outgoingChangeNotifier;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await ExecuteInternalAsync();
            }
            catch (Exception exception)
            {
                HandleException(exception, out bool shouldThrow);
                if (shouldThrow)
                {
                    throw;
                }
            }
        }

        protected abstract Task ExecuteInternalAsync();

        protected virtual void HandleException(Exception exception, out bool shouldThrow)
        {
            Console.WriteLine(exception);
            shouldThrow = true;
        }
    }
}