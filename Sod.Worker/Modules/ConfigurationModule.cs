using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Autofac;
using Microsoft.Extensions.Configuration;
using Sod.Infrastructure.Satel.State.Events.Mqtt;

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

            builder
                .Register(ctx =>
                {
                    var cfg = ctx.Resolve<IConfigurationRoot>();
                    var mappings = 
                        cfg
                            .GetSection("Satel:OutgoingChangeMappings")
                            .GetChildren()
                            .Select(x => x.Get<OutgoingChangeMapping>());
                    return new OutgoingChangeMappings(mappings);
                })
                .As<OutgoingChangeMappings>()
                .SingleInstance();
        }
    }
}