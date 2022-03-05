namespace Sod.Model.Events.Incoming.Events
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