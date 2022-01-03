﻿using System.Threading.Tasks;

namespace Sod.Infrastructure.Satel.State.Events.Incoming
{
    public interface IEventHandler
    {
        Task Handle(IncomingEvent incomingEvent);
    }
}