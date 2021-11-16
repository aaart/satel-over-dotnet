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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected abstract Task ExecuteInternalAsync();
    }
}