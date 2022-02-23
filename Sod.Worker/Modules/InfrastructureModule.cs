using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.Socket;
using Sod.Infrastructure.State.Events.Incoming;
using Sod.Infrastructure.State.Events.Outgoing;
using Sod.Infrastructure.State.Events.Outgoing.Mqtt;
using Sod.Infrastructure.State.Handlers;
using Sod.Infrastructure.State.Handlers.Notifications;
using Sod.Infrastructure.State.Handlers.OutputsUpdate;
using Sod.Infrastructure.State.Handlers.StateRead;
using Sod.Infrastructure.State.Handlers.StorageUpdate;
using Sod.Infrastructure.State.Tasks;
using Sod.Infrastructure.Storage;
using StackExchange.Redis;
using Constants = Sod.Infrastructure.Constants;

namespace Sod.Worker.Modules
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .Register(ctx =>
                {
                    var connString = ctx.Resolve<IConfigurationRoot>().GetConnectionString("Redis");
                    return ConnectionMultiplexer.Connect(connString);
                })
                .As<ConnectionMultiplexer>()
                .SingleInstance();

            builder
                .Register(ctx => ctx.Resolve<ConnectionMultiplexer>().GetDatabase(0))
                .As<IDatabaseAsync>()
                .OnActivated(evnt =>
                {
                    var db = evnt.Instance;
                    IEnumerable<(RedisKey key, RedisValue redisValue)> valueTuples = new[]
                    {
                        Constants.Store.InputsState,
                        Constants.Store.OutputsState
                    }.Select(x => (new RedisKey(x), db.StringGet(new RedisKey(x))));

                    foreach (var tuple in valueTuples)
                    {
                        if (!tuple.redisValue.HasValue)
                        {
                            db.StringSet(tuple.key, new RedisValue(JsonConvert.SerializeObject(Enumerable.Repeat(false, 128).ToArray())));
                        }
                    }
                })
                .SingleInstance();
            builder
                .RegisterType<RedisStore>()
                .As<IStore>()
                .OnActivated(args =>
                {
                    var keysToSet = new[]
                    {
                        Constants.Store.InputsState,
                        Constants.Store.OutputsState
                    };

                    foreach (var keyToSet in keysToSet)
                    {
                        args.Instance.SetAsync(keyToSet, Enumerable.Repeat(false, 128).ToArray()).Wait();
                    }
                })
                .SingleInstance();

            builder
                .Register(ctx =>
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
                .Register(ctx => new MqttFactory().CreateManagedMqttClient())
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
                    client.UseApplicationMessageReceivedHandler(x =>
                    {
                        broker.Process(new IncomingEvent(x.ApplicationMessage.Topic, Encoding.UTF8.GetString(x.ApplicationMessage.Payload)));
                    });
                });
            
            builder.RegisterType<MqttOutgoingEventPublisher>().As<IOutgoingEventPublisher>().SingleInstance();

            builder.RegisterType<RedisTaskQueue>().As<ITaskQueue>().SingleInstance();
            builder
                .RegisterType<TaskPlanner>()
                .As<ITaskPlanner>()
                .SingleInstance();
            builder.RegisterType<HandlerFactory>().As<IHandlerFactory>().SingleInstance();
            
            builder.RegisterType<ReadInputsStateHandler>().AsSelf().SingleInstance();
            builder.RegisterType<ReadOutputsStateHandler>().AsSelf().SingleInstance();
            builder.RegisterType<UpdateStorageStateHandler>().AsSelf().SingleInstance();
            builder.RegisterType<UpdateOutputsStateHandler>().AsSelf().SingleInstance();
            builder.RegisterType<InputsChangeNotificationStateHandler>().AsSelf().SingleInstance();
            builder.RegisterType<OutputsChangeNotificationStateHandler>().AsSelf().SingleInstance();
            builder.RegisterType<QueueProcessor>().As<IQueueProcessor>().SingleInstance();
            builder.RegisterType<Loop>().AsSelf();
        }
    }
}