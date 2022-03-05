using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sod.Model.Tasks.Handlers
{
    public interface ITaskHandler
    {
        Task<IEnumerable<SatelTask>> Handle(SatelTask data);

        object Prop => new object();
    }
}