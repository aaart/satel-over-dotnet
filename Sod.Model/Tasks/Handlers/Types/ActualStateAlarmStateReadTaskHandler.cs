using Sod.Infrastructure.Satel.Communication;
using Sod.Model.DataStructures;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types
{
    public class ActualStateAlarmStateReadTaskHandler : BaseHandler<ActualStateAlarmStateReadTask>
    {
        private readonly IStore _store;
        private readonly IManipulator _manipulator;

        public ActualStateAlarmStateReadTaskHandler(IStore store, IManipulator manipulator)
        {
            _store = store;
            _manipulator = manipulator;
        }
        
        protected override async Task<IEnumerable<SatelTask>> Handle(ActualStateAlarmStateReadTask data)
        {
            var armed = await _store.GetAsync<bool[]>(Constants.Store.ArmedPartitions);
            //return armed.Any(x => x) ? new []{ new SatelTask() }
            return Enumerable.Empty<SatelTask>();
        }
    }
}