using System;
using System.IO;
using System.Linq;
using Autofac;
using Microsoft.Extensions.Configuration;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.Events.Incoming;
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

            builder.RegisterType<OutputEnqueueUpdateStateHandler>().Named<IEventHandler>(typeof(OutputEnqueueUpdateStateHandler).FullName!).InstancePerDependency();
            builder.RegisterType<PairedOutputEnqueueStateUpdateHandler>().Named<IEventHandler>(typeof(PairedOutputEnqueueStateUpdateHandler).FullName!).InstancePerDependency();
            
            builder
                .Register(ctx =>
                {
                    var cfg = ctx.Resolve<IConfigurationRoot>();
                    var mappings =
                        cfg
                            .GetSection("Satel:Topic2IncomingEventHandlerMappings")
                            .GetChildren()
                            .Select(x => (x["Topic"], x["Handler"], int.Parse(x["IOIndex"])));

                    return new EventHandlerMappings(mappings.Select(x => (x.Item1, ctx.ResolveNamed<IEventHandler>(x.Item2, new NamedParameter("ioIndex", x.Item3)))));
                })
                .As<EventHandlerMappings>()
                .SingleInstance();
        }
    }
}