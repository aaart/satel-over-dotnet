using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MQTTnet.Extensions.ManagedClient;
using Sod.Model.Events.Incoming;
using Sod.Model.Processing;

namespace Sod.Worker;

public class Worker(
    ILoop loop,
    IBroker broker,
    IManagedMqttClient client,
    ManagedMqttClientOptions options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await SetupAsync();
        await loop.ExecuteAsync(stoppingToken);
    }

    private async Task SetupAsync()
    {
        await client.StartAsync(options);
        client.ApplicationMessageReceivedAsync += args => broker.Process(args.ApplicationMessage.Topic, Encoding.UTF8.GetString(args.ApplicationMessage.PayloadSegment));
    }
}