using System.Collections.Generic;

namespace Sod.Infrastructure.State.Loop
{
    public interface IStepCollection : IStep, IEnumerable<IStep>
    {
    }
}