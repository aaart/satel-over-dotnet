using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.State.Handlers
{
    public interface IHandler
    {
        Task Handle(IReadOnlyDictionary<string, object> parameters);
    }
}