using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.CommonTypes;
using Sod.Model.DataStructures;
using Sod.Model.Tasks.Handlers.Policies;
using Sod.Model.Tasks.Types;
using Sod.Model.Tools;

namespace Sod.Model.Tasks.Handlers.Types
{
    public class ActualStateBinaryIOReadTaskHandler : BaseHandler<ActualStateBinaryIOReadTask>
    {
        private readonly IStore _store;
        private readonly IManipulator _manipulator;
        private readonly IPostReadPolicy _postReadPolicy;

        public ActualStateBinaryIOReadTaskHandler(IStore store, IManipulator manipulator, IPostReadPolicy postReadPolicy)
        {
            _store = store;
            _manipulator = manipulator;
            _postReadPolicy = postReadPolicy;
        }

        protected override async Task<IEnumerable<SatelTask>> Handle(ActualStateBinaryIOReadTask data)
        {
            var (status, actualState) = await ManipulatorMethod(data.Method);
            ValidateStatus(status);

            var persistedState = await _store.GetAsync<bool[]>(data.PersistedStateKey);
            ValidateState(persistedState, actualState);

            var changes = IO.ExtractIOChanges(persistedState, actualState);

            return _postReadPolicy.Apply(changes, data.PersistedStateKey, actualState, data.OutgoingEventType);
        }

        private async Task<(CommandStatus, bool[])> ManipulatorMethod(IOReadManipulatorMethod method)
        {
            switch (method)
            {
                case IOReadManipulatorMethod.Inputs:
                    return await _manipulator.ReadInputs();
                case IOReadManipulatorMethod.Outputs:
                    return await _manipulator.ReadOutputs();
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }

        private void ValidateStatus(CommandStatus status)
        {
            if (status != CommandStatus.Processed)
            {
                throw new InvalidOperationException($"{status} status cannot be processed.");
            }
        }

        private void ValidateState(bool[] persistedState, bool[] satelState)
        {
            if (persistedState.Length != satelState.Length)
            {
                throw new InvalidOperationException($"Current satel state and persisted state have different lengths!");
            }
        }
    }
}