using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.Events.Incoming;
using Sod.Model.Events.Incoming.Events.Handlers;
using Sod.Model.Events.Outgoing.Mqtt;
using Sod.Model.Processing;

namespace Sod.Worker.Modules
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(_ => new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.local.json", optional: true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                    .Build())
                .As<IConfigurationRoot>()
                .SingleInstance();
            
            builder.RegisterConfiguration<LoopOptions>("Loop");
            builder.RegisterConfiguration<MqttOptions>("Mqtt");
            builder.RegisterConfiguration<SatelConnectionOptions>("Satel");
            builder.RegisterConfiguration<SatelUserCodeOptions>("Satel");

            builder
                .Register(ctx =>
                {
                    var cfg = ctx.Resolve<IConfigurationRoot>();
                    var mappings = 
                        cfg
                            .GetSection("Satel:OutgoingEventMappings")
                            .GetChildren()
                            .Select(x => x.Get<OutgoingEventMapping>());
                    return new OutgoingEventMappings(mappings);
                })
                .As<OutgoingEventMappings>()
                .SingleInstance();

            builder.RegisterType<OutputEnqueueUpdateStateHandler>().As<IEventHandler>().InstancePerDependency();

            builder
                .Register(ctx =>
                {
                    var cfg = ctx.Resolve<IConfigurationRoot>();
                    IEnumerable<(string topic, bool notify, int ioIndex)> mappings =
                        cfg
                            .GetSection("Satel:IncomingEventMappings")
                            .GetChildren()
                            .Select(x => (x["Topic"], bool.Parse(x["Notify"]), int.Parse(x["IOIndex"])));

                    return new EventHandlerMappings(
                        mappings.Select(x => (x.topic, ctx.Resolve<IEventHandler>(new NamedParameter("ioIndex", x.ioIndex), new NamedParameter("notify", x.notify)))));
                })
                .As<EventHandlerMappings>()
                .SingleInstance();
        }
    }
}