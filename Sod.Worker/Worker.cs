using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Sod.Infrastructure.State.Tasks;

namespace Sod.Worker
{
    public class Worker : BackgroundService
    {
        private readonly Loop _loop;

        public Worker(Loop loop)
        {
            _loop = loop;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _loop.ExecuteAsync(stoppingToken);
        }

        
    }
}