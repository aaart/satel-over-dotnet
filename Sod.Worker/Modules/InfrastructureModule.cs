using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Autofac;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.Socket;
using Sod.Model;
using Sod.Model.DataStructures;
using Sod.Model.Events.Incoming;
using Sod.Model.Events.Incoming.Events;
using Sod.Model.Events.Outgoing;
using Sod.Model.Events.Outgoing.Mqtt;
using Sod.Model.Processing;
using Sod.Model.Tasks.Handlers;
using Sod.Model.Tasks.Handlers.Types;
using StackExchange.Redis;

namespace Sod.Worker.Modules
{
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
                    args.Instance.SetAsync(Constants.Store.ArmedPartitions, Enumerable.Repeat(true, 32).ToArray());
                })
                .SingleInstance();
            
            builder
                .Register(_ =>
                {
                    var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    return socket;
                })
                .As<Socket>()
                .SingleInstance()
                .OnActivated(args =>
                {
                    var cfg = args.Context.Resolve<SatelConnectionOptions>();
                    args.Instance.Connect(cfg.Address, cfg.Port);
                });

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
                    {
                        optionsBuilder.WithTls(x =>
                        {
                            X509Certificate2 caCrt = new X509Certificate2(File.ReadAllBytes(cfg.CrtPath));
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
                })
                .As<IMqttClientOptions>()
                .SingleInstance();

            builder
                .Register(ctx =>
                    new ManagedMqttClientOptionsBuilder()
                        .WithAutoReconnectDelay(TimeSpan.FromSeconds(2))
                        .WithClientOptions(ctx.Resolve<IMqttClientOptions>())
                        .Build())
                .As<IManagedMqttClientOptions>()
                .SingleInstance();

            builder.RegisterType<Broker>().As<IBroker>().SingleInstance();

            builder
                .Register(_ => new MqttFactory().CreateManagedMqttClient())
                .As<IApplicationMessagePublisher>()
                .As<IApplicationMessageReceiver>()
                .SingleInstance()
                .OnActivated(async activatedEventArgs =>
                {
                    var client = activatedEventArgs.Instance;
                    await client.StartAsync(activatedEventArgs.Context.Resolve<IManagedMqttClientOptions>());
                    var mappings = activatedEventArgs.Context.Resolve<EventHandlerMappings>();
                    foreach (var topic in mappings.Topics)
                    {
                        await client.SubscribeAsync(topic);
                    }

                    var broker = activatedEventArgs.Context.Resolve<IBroker>();
                    client.UseApplicationMessageReceivedHandler(x => { broker.Process(new IncomingEvent(x.ApplicationMessage.Topic, Encoding.UTF8.GetString(x.ApplicationMessage.Payload))); });
                });

            builder.RegisterType<MqttOutgoingEventPublisher>().As<IOutgoingEventPublisher>().SingleInstance();

            builder.RegisterType<InMemoryTaskQueue>().As<ITaskQueue>().SingleInstance();
            builder
                .RegisterType<TaskPlanner>()
                .As<ITaskPlanner>()
                .SingleInstance();
            builder.RegisterType<HandlerFactory>().As<IHandlerFactory>().SingleInstance();

            builder.RegisterType<ActualStateReadTaskHandler>().AsSelf().SingleInstance();
            builder.RegisterType<PersistedStateUpdateTaskHandler>().AsSelf().SingleInstance();
            builder.RegisterType<ActualStateOutputsUpdateTaskHandler>().AsSelf().SingleInstance();
            builder.RegisterType<ActualStateChangedNotificationTaskHandler>().AsSelf().SingleInstance();
            builder.RegisterType<QueueProcessor>().As<IQueueProcessor>().SingleInstance();
            builder.RegisterType<Loop>().AsSelf();
        }
    }
}