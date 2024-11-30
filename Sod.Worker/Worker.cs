using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MQTTnet.Extensions.ManagedClient;
using Sod.Model.Events.Incoming;
using Sod.Model.Processing;

namespace Sod.Worker;

public class Worker : BackgroundService
{
    private readonly ILoop _loop;
    private readonly IBroker _broker;
    private readonly IManagedMqttClient _client;
    private readonly ManagedMqttClientOptions _options;

    public Worker(
        ILoop loop,
        IBroker broker,
        IManagedMqttClient client,
        ManagedMqttClientOptions options)
    {
        _loop = loop;
        _broker = broker;
        _client = client;
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await SetupAsync();
        await _loop.ExecuteAsync(stoppingToken);
    }

    private async Task SetupAsync()
    {
        await _client.StartAsync(_options);
        _client.ApplicationMessageReceivedAsync += args => _broker.Process(args.ApplicationMessage.Topic, Encoding.UTF8.GetString(args.ApplicationMessage.PayloadSegment));
    }
}