using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sod.Model.Tasks.Types;

namespace Sod.Model.Tasks.Handlers.Types
{
    public class PersistedStateAlarmStateReadTaskHandler : BaseHandler<PersistedStateAlarmStateReadTask>
    {
        protected override Task<IEnumerable<SatelTask>> Handle(PersistedStateAlarmStateReadTask data)
        {
            return Task.FromResult(Enumerable.Empty<SatelTask>());
        }
    }
}