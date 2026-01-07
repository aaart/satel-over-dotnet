namespace Sod.Model.Tasks;

public class ActualStateGlobalBroadcastTask(string timestamp) : BaseSatelTask
{
    public string Timestamp { get; } = timestamp;
}
