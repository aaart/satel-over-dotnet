// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

class Program
{
    static async Task Main(string[] args)
    {
        var msg = args.FirstOrDefault() ?? DateTime.Now.ToString("yy-MM-dd HH:mm.ss UTCzz");
        Console.WriteLine(msg);

        var cfg = CreateConfiguration();
        var mqttOpt = CreateMqttClientOptions(cfg);
        var client = new MqttFactory().CreateMqttClient();
        await client.ConnectAsync(mqttOpt);
        var res = await client.PublishAsync("mqttconsoletest", msg);
        Console.WriteLine($"reason: {res.ReasonString}");

        var subRes = await client.SubscribeAsync("test_topic");
        
    }


    private static IConfigurationRoot CreateConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.local.json", optional: true)
            .Build();
    }

    private static IMqttClientOptions CreateMqttClientOptions(IConfigurationRoot cfg)
    {
        
        var optionsBuilder = new MqttClientOptionsBuilder()
            .WithCredentials(cfg.GetValue<string>("Mqtt:User"), cfg.GetValue<string>("Mqtt:Password"))
            .WithTcpServer(cfg.GetValue<string>("Mqtt:Host"), cfg.GetValue<int>("Mqtt:Port"));
        if (cfg.GetValue<bool>("Mqtt:UseTLS"))
        {
            optionsBuilder.WithTls(x =>
            {
                X509Certificate2 caCrt = new X509Certificate2(File.ReadAllBytes("ca.crt"));
                x.UseTls = true;
                x.SslProtocol = System.Security.Authentication.SslProtocols.Tls12;
                x.CertificateValidationHandler = (certContext) =>
                {
                    X509Chain chain = new X509Chain();
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
                    chain.ChainPolicy.VerificationTime = DateTime.Now;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 0);
                                
                    chain.ChainPolicy.CustomTrustStore.Add(caCrt);
                    chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;

                    // convert provided X509Certificate to X509Certificate2
                    var x5092 = new X509Certificate2(certContext.Certificate);

                    return chain.Build(x5092);
                };
            });
        }
                    
        return optionsBuilder.Build();
    }
}