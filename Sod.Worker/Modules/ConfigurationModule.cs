using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Microsoft.Extensions.Configuration;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.Events.Incoming;
using Sod.Model.Events.Outgoing.Mqtt;
using Sod.Model.Processing;

namespace Sod.Worker.Modules;

public class ConfigurationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        RegisterConfigurationRoot(builder);
        RegisterOptions(builder);
        RegisterOutgoingEventMappings(builder);
        RegisterIncomingEventMappings(builder);
    }

    private static void RegisterConfigurationRoot(ContainerBuilder builder)
    {
        builder.Register(_ => BuildConfiguration())
            .As<IConfigurationRoot>()
            .SingleInstance();
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var basePath = Directory.Exists("/workspace")
            ? "/workspace"
            : Directory.GetCurrentDirectory();

        return new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.local.json", optional: true)
            .Build();
    }

    private static void RegisterOptions(ContainerBuilder builder)
    {
        builder.RegisterConfiguration<LoopOptions>("Loop");
        builder.RegisterConfiguration<MqttOptions>("Mqtt");
        builder.RegisterConfiguration<SatelConnectionOptions>("Satel");
        builder.RegisterConfiguration<SatelUserCodeOptions>("Satel");
    }

    private static void RegisterOutgoingEventMappings(ContainerBuilder builder)
    {
        builder.Register(ctx =>
            {
                var configuration = ctx.Resolve<IConfigurationRoot>();
                var mappings = GetOutgoingEventMappings(configuration);
                return new OutgoingEventMappings(mappings);
            })
            .As<OutgoingEventMappings>()
            .SingleInstance();
    }

    private static IEnumerable<OutgoingEventMapping> GetOutgoingEventMappings(IConfiguration configuration)
    {
        return configuration
            .GetSection("Satel:OutgoingEventMappings")
            .GetChildren()
            .Select(section => section.Get<OutgoingEventMapping>());
    }

    private static void RegisterIncomingEventMappings(ContainerBuilder builder)
    {
        builder.RegisterType<StateChangeDispatcher>()
            .AsSelf()
            .InstancePerDependency();

        builder.Register(ctx =>
            {
                var configuration = ctx.Resolve<IConfigurationRoot>();
                var mappings = GetIncomingEventMappings(configuration);
                var handlers = CreateStateChangeDispatchers(ctx, mappings);
                return new EventHandlerMappings(handlers);
            })
            .As<EventHandlerMappings>()
            .SingleInstance();
    }

    private static IEnumerable<IncomingEventMappingConfig> GetIncomingEventMappings(IConfiguration configuration)
    {
        return configuration
            .GetSection("Satel:IncomingEventMappings")
            .GetChildren()
            .Select(section => new IncomingEventMappingConfig(
                Enum.Parse<IncomingEventType>(section["Type"]!),
                section["Topic"]!,
                bool.Parse(section["Notify"]!),
                int.Parse(section["IOIndex"]!)));
    }

    private static IEnumerable<(string topic, IStateChangeDispatcher dispatcher)> CreateStateChangeDispatchers(
        IComponentContext context,
        IEnumerable<IncomingEventMappingConfig> mappings)
    {
        return mappings.Select(mapping => (
            mapping.Topic,
            CreateStateChangeDispatcher(context, mapping)
        ));
    }

    private static IStateChangeDispatcher CreateStateChangeDispatcher(
        IComponentContext context,
        IncomingEventMappingConfig mapping)
    {
        return context.Resolve<StateChangeDispatcher>(
            new NamedParameter("incomingEventType", mapping.EventType),
            new NamedParameter("ioIndex", mapping.IoIndex),
            new NamedParameter("notify", mapping.Notify));
    }

    private record IncomingEventMappingConfig(
        IncomingEventType EventType,
        string Topic,
        bool Notify,
        int IoIndex);
}