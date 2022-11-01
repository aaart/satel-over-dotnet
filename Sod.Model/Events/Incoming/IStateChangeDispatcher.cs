namespace Sod.Model.Events.Incoming;

public interface IStateChangeDispatcher
{
    Task HandleAsync(string payload);
}