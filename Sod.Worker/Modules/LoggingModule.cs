using System;
using System.Linq;
using Autofac;
using Autofac.Core.Resolving.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Sod.Infrastructure.Capabilities;
using Sod.Infrastructure.Satel.Communication;

namespace Sod.Worker.Modules
{
    public class LoggingModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .Register(ctx => new LoggerConfiguration().ReadFrom.Configuration(ctx.Resolve<IConfigurationRoot>()))
                .As<LoggerConfiguration>()
                .SingleInstance();

            builder
                .Register(ctx => ctx.Resolve<LoggerConfiguration>().CreateLogger())
                .As<Serilog.ILogger>()
                .SingleInstance();

            builder
                .Register(ctx => new SerilogLoggerFactory(ctx.Resolve<Serilog.ILogger>()))
                .AsSelf()
                .InstancePerDependency();
            
            builder.ComponentRegistryBuilder.Registered += (_, args) =>
            {
                args.ComponentRegistration.PipelineBuilding += (_ , pipeline) =>
                {
                    pipeline.Use(new LoggingMiddleware());
                };
            };
        }
        
        public class LoggingMiddleware : IResolveMiddleware
        {
            public void Execute(ResolveRequestContext ctx, Action<ResolveRequestContext> next)
            {
                next(ctx);
                var instance = ctx.Instance;
                if (instance is ILoggingCapability loggingCapability)
                {
                    loggingCapability.Logger = ctx.Resolve<SerilogLoggerFactory>().CreateLogger(instance.GetType());
                }
            }

            public PipelinePhase Phase => PipelinePhase.Activation;
        }
    }
}