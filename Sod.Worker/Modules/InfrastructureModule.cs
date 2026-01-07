using System;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Autofac;
using Autofac.Core;
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
    private const int InputOutputCount = 128;
    private const int PartitionCount = 32;

    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        RegisterDataStore(builder);
        RegisterSocketComponents(builder);
        RegisterMqttComponents(builder);
        RegisterTaskProcessing(builder);
    }

    private static void RegisterDataStore(ContainerBuilder builder)
    {
        builder
            .RegisterType<InMemoryStore>()
            .As<IStore>()
            .OnActivated(InitializeStoreState)
            .SingleInstance();
    }

    private static void InitializeStoreState(IActivatedEventArgs<InMemoryStore> args)
    {
        args.Instance.SetAsync(Constants.Store.InputsState, CreateBoolArray(InputOutputCount, false));
        args.Instance.SetAsync(Constants.Store.OutputsState, CreateBoolArray(InputOutputCount, false));
        args.Instance.SetAsync(Constants.Store.ArmedPartitions, CreateBoolArray(PartitionCount, true));
        args.Instance.SetAsync(Constants.Store.TriggeredPartitions, CreateBoolArray(PartitionCount, true));
        args.Instance.SetAsync(Constants.Store.SuppressedPartitions, CreateBoolArray(PartitionCount, true));
    }

    private static bool[] CreateBoolArray(int count, bool initialValue) =>
        Enumerable.Repeat(initialValue, count).ToArray();

    private static void RegisterSocketComponents(ContainerBuilder builder)
    {
        builder.RegisterType<SocketConnection>().As<ISocketConnection>().SingleInstance();
        builder.RegisterType<SocketSender>().As<ISocketSender>().SingleInstance();
        builder.RegisterType<SocketReceiver>().As<ISocketReceiver>().SingleInstance();
        builder.RegisterType<GenericCommunicationInterface>().AsSelf().SingleInstance();
        builder.RegisterType<Manipulator>().As<IManipulator>().SingleInstance();
    }

    private static void RegisterMqttComponents(ContainerBuilder builder)
    {
        builder.Register(CreateMqttClientOptions).As<MqttClientOptions>().SingleInstance();
        builder.Register(CreateManagedMqttClientOptions).As<ManagedMqttClientOptions>().SingleInstance();
        builder.RegisterType<Broker>().As<IBroker>().SingleInstance();

        builder
            .Register(_ => new MqttFactory().CreateManagedMqttClient())
            .As<IManagedMqttClient>()
            .SingleInstance()
            .OnActivated(SubscribeToMqttTopics);

        builder
            .Register(ctx => ctx.Resolve<IManagedMqttClient>().InternalClient)
            .As<IMqttClient>()
            .SingleInstance();
        
        builder
            .RegisterType<MqttOutgoingEventPublisher>()
            .As<IOutgoingEventPublisher>()
            .OnActivated(ConfigureOutgoingEventPublisher)
            .SingleInstance();
    }

    private static MqttClientOptions CreateMqttClientOptions(IComponentContext ctx)
    {
        var config = ctx.Resolve<MqttOptions>();
        var optionsBuilder = new MqttClientOptionsBuilder()
            .WithCredentials(config.User, config.Password)
            .WithTcpServer(config.Host, config.Port);

        if (config.CrtPath != null)
        {
            var caCertificate = X509CertificateLoader.LoadCertificate(File.ReadAllBytes(config.CrtPath));
            optionsBuilder.WithTlsOptions(opt =>
            {
                opt.UseTls();
                opt.WithSslProtocols(SslProtocols.Tls12 | SslProtocols.Tls13);
                opt.WithCertificateValidationHandler(context => ValidateCertificate(context, caCertificate));
            });
        }

        return optionsBuilder.Build();
    }

    private static bool ValidateCertificate(
        MqttClientCertificateValidationEventArgs context,
        X509Certificate2 caCertificate)
    {
        var chain = new X509Chain
        {
            ChainPolicy =
            {
                RevocationMode = X509RevocationMode.NoCheck,
                RevocationFlag = X509RevocationFlag.ExcludeRoot,
                VerificationFlags = X509VerificationFlags.NoFlag,
                VerificationTime = DateTime.Now,
                UrlRetrievalTimeout = TimeSpan.Zero,
                TrustMode = X509ChainTrustMode.CustomRootTrust
            }
        };

        chain.ChainPolicy.CustomTrustStore.Add(caCertificate);
        var certificate = new X509Certificate2(context.Certificate);

        return chain.Build(certificate);
    }

    private static ManagedMqttClientOptions CreateManagedMqttClientOptions(IComponentContext ctx)
    {
        return new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(2))
            .WithClientOptions(ctx.Resolve<MqttClientOptions>())
            .Build();
    }

    private static async void SubscribeToMqttTopics(IActivatedEventArgs<IManagedMqttClient> args)
    {
        var client = args.Instance;
        var mappings = args.Context.Resolve<EventHandlerMappings>();

        foreach (var topic in mappings.Topics)
        {
            await client.SubscribeAsync(topic);
        }
    }

    private static void ConfigureOutgoingEventPublisher(
        IActivatedEventArgs<MqttOutgoingEventPublisher> args)
    {
        var options = args.Context.Resolve<MqttOptions>();
        args.Instance.Retain(options.Retain).QoS(options.QoS);
    }

    private static void RegisterTaskProcessing(ContainerBuilder builder)
    {
        builder.RegisterType<InMemoryTaskQueue>().As<ITaskQueue>().SingleInstance();
        builder.RegisterType<TaskPlanner>().As<ITaskPlanner>().SingleInstance();
        builder.RegisterType<HandlerFactory>().As<IHandlerFactory>().SingleInstance();

        var handlerTypes = typeof(BaseHandler<>).Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo<ITaskHandler>())
            .ToArray();

        builder.RegisterTypes(handlerTypes).AsSelf().SingleInstance();

        builder.RegisterType<QueueProcessor>().As<IQueueProcessor>().SingleInstance();
        builder.RegisterType<Loop>().As<ILoop>().SingleInstance();
        builder.RegisterType<LoopIteration>().As<ILoopIteration>().SingleInstance();
        builder.RegisterType<InfraLevelExceptionHandlingPolicy>()
            .As<ILoopIterationExceptionHandlingPolicy>()
            .SingleInstance();
    }
}