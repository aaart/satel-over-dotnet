using System;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Autofac;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.Socket;
using Sod.Model;
using Sod.Model.DataStructures;
using Sod.Model.Events.Incoming;
using Sod.Model.Events.Outgoing;
using Sod.Model.Events.Outgoing.Mqtt;
using Sod.Model.Processing;
using Sod.Model.Tasks.Handlers;

namespace Sod.Worker.Modules;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder
            .RegisterType<InMemoryStore>()
            .As<IStore>()
            .OnActivated(args =>
            {
                args.Instance.SetAsync(Constants.Store.InputsState, Enumerable.Repeat(false, 128).ToArray());
                args.Instance.SetAsync(Constants.Store.OutputsState, Enumerable.Repeat(false, 128).ToArray());
                args.Instance.SetAsync(Constants.Store.ArmedPartitions, Enumerable.Repeat(true, 32).ToArray()); // most probably will be set to false after start
                args.Instance.SetAsync(Constants.Store.TriggeredPartitions, Enumerable.Repeat(true, 32).ToArray());
            })
            .SingleInstance();

        builder
            .RegisterType<SocketConnection>()
            .As<ISocketConnection>()
            .SingleInstance();

        builder.RegisterType<SocketSender>().As<ISocketSender>().SingleInstance();
        builder.RegisterType<SocketReceiver>().As<ISocketReceiver>().SingleInstance();
        builder.RegisterType<GenericCommunicationInterface>().AsSelf().SingleInstance();

        builder.RegisterType<Manipulator>().As<IManipulator>().SingleInstance();

        builder
            .Register(ctx =>
            {
                var cfg = ctx.Resolve<MqttOptions>();

                var optionsBuilder = new MqttClientOptionsBuilder()
                    .WithCredentials(cfg.User, cfg.Password)
                    .WithTcpServer(cfg.Host, cfg.Port);
                if (cfg.CrtPath != null)
                    optionsBuilder.WithTlsOptions(opt =>
                    {
                        var caCrt = X509CertificateLoader.LoadCertificate(File.ReadAllBytes(cfg.CrtPath));
                        opt
                            .UseTls()
                            .WithSslProtocols(SslProtocols.Tls12 | SslProtocols.Tls13)
                            .WithCertificateValidationHandler(certContext =>
                            {
                                var chain = new X509Chain();
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
                            });
                    });

                return optionsBuilder.Build();
            })
            .As<MqttClientOptions>()
            .SingleInstance();

        builder
            .Register(ctx =>
                new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(2))
                    .WithClientOptions(ctx.Resolve<MqttClientOptions>())
                    .Build())
            .As<ManagedMqttClientOptions>()
            .SingleInstance();

        builder.RegisterType<Broker>().As<IBroker>().SingleInstance();

        builder
            .Register(_ => new MqttFactory().CreateManagedMqttClient())
            .As<IManagedMqttClient>()
            .SingleInstance()
            .OnActivated(async activatedEventArgs =>
            {
                var client = activatedEventArgs.Instance;
                var mappings = activatedEventArgs.Context.Resolve<EventHandlerMappings>();
                foreach (var topic in mappings.Topics) await client.SubscribeAsync(topic);
            });

        builder
            .Register(ctx => ctx.Resolve<IManagedMqttClient>().InternalClient)
            .As<IMqttClient>()
            .SingleInstance();
        
        builder
            .RegisterType<MqttOutgoingEventPublisher>()
            .As<IOutgoingEventPublisher>()
            .OnActivated(activatedEventArgs =>
            {
                var opt = activatedEventArgs.Context.Resolve<MqttOptions>();
                activatedEventArgs.Instance.Retain(opt.Retain).QoS(opt.QoS);
            })
            .SingleInstance();

        builder.RegisterType<InMemoryTaskQueue>().As<ITaskQueue>().SingleInstance();
        builder.RegisterType<TaskPlanner>().As<ITaskPlanner>().SingleInstance();
        builder.RegisterType<HandlerFactory>().As<IHandlerFactory>().SingleInstance();

        builder.RegisterTypes(typeof(BaseHandler<>).Assembly.GetTypes().Where(x => x.IsAssignableTo<ITaskHandler>()).ToArray()).AsSelf().SingleInstance();

        builder.RegisterType<QueueProcessor>().As<IQueueProcessor>().SingleInstance();
        builder.RegisterType<Loop>().As<ILoop>().SingleInstance();
        builder.RegisterType<LoopIteration>().As<ILoopIteration>().SingleInstance();
        builder.RegisterType<InfraLevelExceptionHandlingPolicy>().As<ILoopIterationExceptionHandlingPolicy>().SingleInstance();
    }
}