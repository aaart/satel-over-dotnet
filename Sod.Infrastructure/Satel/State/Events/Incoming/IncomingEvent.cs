﻿namespace Sod.Infrastructure.Satel.State.Events.Incoming
{
    public class IncomingEvent
    {
        public IncomingEvent(string topic, string payload)
        {
            Topic = topic;
            Payload = payload;
        }

        public string Topic { get; }
        public string Payload { get; }
    }
}