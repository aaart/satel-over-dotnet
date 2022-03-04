﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Storage;
using Sod.Infrastructure.Storage.TaskTypes.IOStateRead;
using Sod.Infrastructure.Storage.TaskTypes.Notifications;
using Sod.Infrastructure.Storage.TaskTypes.StorageUpdate;

namespace Sod.Infrastructure.State.Tasks.Handlers.IOStateRead
{
    public class ReadIOStateTaskHandler : BaseHandler<ReadStateTask>
    {
        private readonly IStore _store;
        private readonly IManipulator _manipulator;

        public ReadIOStateTaskHandler(IStore store, IManipulator manipulator)
        {
            _store = store;
            _manipulator = manipulator;
        }

        protected override async Task<IEnumerable<SatelTask>> Handle(ReadStateTask data)
        {
            var (status, satelState) = await ManipulatorMethod(data.Method);
            ValidateStatus(status);

            var persistedState = await _store.GetAsync<bool[]>(data.PersistedStateKey);
            ValidateState(persistedState, satelState);

            var changes = new List<IOState>();
            for (int i = 0; i < persistedState.Length; i++)
            {
                if (persistedState[i] != satelState[i])
                {
                    changes.Add(new IOState { Index = i + 1, Value = satelState[i] });
                }
            }

            if (changes.Any())
            {
                var t1 = new StorageUpdateTask(data.PersistedStateKey, satelState);
                var t2 = new IOChangeNotificationTask(changes, data.OutgoingEventType); 
                return new SatelTask[] { t1, t2 };
            }

            return Enumerable.Empty<SatelTask>();
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