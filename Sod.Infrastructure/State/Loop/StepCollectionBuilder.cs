using Sod.Infrastructure.Store;

namespace Sod.Infrastructure.State.Loop
{
    public class StepCollectionBuilder : IStepCollectionBuilder
    {
        private readonly IStore _store;

        public StepCollectionBuilder(IStore store)
        {
            _store = store;
        }
        
        
    }
}