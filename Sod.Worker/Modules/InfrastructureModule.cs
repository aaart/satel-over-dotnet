using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Autofac;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Implementations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Core;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.Socket;
using Sod.Infrastructure.Satel.State.Events;
using Sod.Infrastructure.Satel.State.Events.Mqtt;
using Sod.Infrastructure.Satel.State.Loop;
using Sod.Infrastructure.Store;
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
                    var cfg = ctx.Resolve<IConfigurationRoot>();
                    var connString = cfg.GetValue<string>("ConnectionStrings:Redis");
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
                        Constants.Store.InputsStateKey,
                        Constants.Store.OutputsStateKey
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
            builder.RegisterType<RedisStore>().As<IStore>().SingleInstance();

            builder
                .Register(ctx =>
                {
                    var cfg = ctx.Resolve<IConfigurationRoot>();
                    var address = cfg.GetValue<string>("Satel:Address");
                    var port = cfg.GetValue<int>("Satel:Port");
                    
                    var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(address, port);
                    return socket;
                })
                .As<Socket>()
                .SingleInstance();
            
            builder.RegisterType<SocketSender>().As<ISocketSender>().SingleInstance();
            builder.RegisterType<SocketReceiver>().As<ISocketReceiver>().SingleInstance();
            
            builder
                .Register(ctx =>
                {
                    var cfg = ctx.Resolve<IConfigurationRoot>();
                    return new Manipulator(ctx.Resolve<ISocketSender>(), ctx.Resolve<ISocketReceiver>(), cfg.GetValue<string>("Satel:UserCode"));
                })
                .As<IManipulator>()
                .SingleInstance();

            builder
                .Register(ctx =>
                {
                    var cfg = ctx.Resolve<IConfigurationRoot>();
                    var optionsBuilder = new MqttClientOptionsBuilder()
                        .WithCredentials(cfg.GetValue<string>("Mqtt:User"), cfg.GetValue<string>("Mqtt:Password"))
                        .WithTcpServer(cfg.GetValue<string>("Mqtt:Host"), cfg.GetValue<int>("Mqtt:Port"));
                    if (cfg.GetValue<bool>("Mqtt:UseTLS"))
                    {
                        optionsBuilder.WithTls();
                    }
                    
                    return optionsBuilder.Build();
                })
                .As<IMqttClientOptions>()
                .SingleInstance();

            builder
                .Register(ctx =>
                {
                    var client = new MqttFactory().CreateMqttClient();
                    client.ConnectAsync(ctx.Resolve<IMqttClientOptions>());  
                    return client;
                })
                .As<IMqttClient>()
                .SingleInstance();
            
            builder.RegisterType<MqttOutgoingChangeNotifier>().As<IOutgoingChangeNotifier>().SingleInstance();
            builder.RegisterType<StepCollectionFactory>().As<IStepCollectionFactory>().SingleInstance();
        }
    }
}