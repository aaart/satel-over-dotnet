using System.Collections.Generic;

namespace Sod.Infrastructure.Satel.State.Loop
{
    public interface IStepCollection : IStep, IEnumerable<IStep>
    {
    }
}