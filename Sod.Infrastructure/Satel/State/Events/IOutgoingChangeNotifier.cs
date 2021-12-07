﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.State.Events
{
    public interface IOutgoingChangeNotifier
    {
        Task<IEnumerable<FailedOutgoingChange>> Publish(OutgoingChange change);
    }
}