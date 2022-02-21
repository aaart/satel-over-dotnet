using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Sod.Infrastructure.Configuration;
using Sod.Infrastructure.State.Events.Incoming;
using Sod.Infrastructure.State.Events.Mqtt;
using Module = Autofac.Module;

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
            
            builder.RegisterOptions<LoopOptions>();
            builder.RegisterOptions<MqttOptions>();

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

            // builder
            //     .RegisterType<OutputDirectUpdateStateHandler>().Named<IEventHandler>("Sod.Infrastructure.Satel.State.Events.Incoming.UpdateOutputStateHandler").InstancePerDependency();
            builder.RegisterType<OutputEnqueueUpdateStateHandler>().Named<IEventHandler>("Sod.Infrastructure.Satel.State.Events.Incoming.OutputEnqueueUpdateStateHandler").InstancePerDependency();
            
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