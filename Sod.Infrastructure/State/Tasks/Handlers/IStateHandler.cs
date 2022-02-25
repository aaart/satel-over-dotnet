using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Tasks.Handlers
{
    public interface IStateHandler
    {
        Task<IEnumerable<SatelTask>> Handle(IReadOnlyDictionary<string, object> parameters);
    }
}