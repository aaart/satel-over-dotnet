using System.Threading.Tasks;
using Sod.Model.DataStructures;

namespace Sod.Model.Processing
{
    public interface ITaskPlanner
    {
        Task Plan(ITaskQueue queue, int iteration);
    }
}