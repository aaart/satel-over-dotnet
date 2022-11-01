namespace Sod.Model.Events.Incoming;

public interface IBroker
{
    Task Process(string topic, string payload);
}