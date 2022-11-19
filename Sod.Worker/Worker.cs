using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Sod.Model.Processing;

namespace Sod.Worker;

public class Worker : BackgroundService
{
    private readonly ILoop _loop;

    public Worker(ILoop loop)
    {
        _loop = loop;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _loop.ExecuteAsync(stoppingToken);
    }
}