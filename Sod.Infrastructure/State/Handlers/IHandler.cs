using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Handlers
{
    public interface IHandler
    {
        Task<IEnumerable<SatelTask>> Handle(IReadOnlyDictionary<string, object> parameters);
    }
}