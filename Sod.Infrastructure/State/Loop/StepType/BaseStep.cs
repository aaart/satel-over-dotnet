using System;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.State.Events;
using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.State.Loop.StepType
{
    public abstract class BaseStep : IStep
    {
        protected IStore Store { get; }
        protected IManipulator Manipulator { get; }
        protected IEventPublisher EventPublisher { get; }

        protected BaseStep(
            IStore store, 
            IManipulator manipulator,
            IEventPublisher eventPublisher)
        {
            Store = store;
            Manipulator = manipulator;
            EventPublisher = eventPublisher;
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