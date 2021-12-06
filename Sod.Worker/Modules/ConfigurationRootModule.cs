using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Sod.Worker.Modules
{
    public class ConfigurationRootModule : Module
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
        }
    }
}