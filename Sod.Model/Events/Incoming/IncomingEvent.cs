namespace Sod.Model.Events.Incoming
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