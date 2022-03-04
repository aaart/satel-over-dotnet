using System.Collections.Generic;
using System.Threading.Tasks;
using Sod.Infrastructure.Storage;

namespace Sod.Infrastructure.State.Tasks.Handlers
{
    public interface ITaskHandler
    {
        Task<IEnumerable<SatelTask>> Handle(SatelTask data);

        object Prop => new object();
    }
}