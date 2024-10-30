namespace Sod.Model.Processing;

public interface ILoop
{
    Task ExecuteAsync(CancellationToken stoppingToken);
}