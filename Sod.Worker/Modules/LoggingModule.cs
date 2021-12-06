using System;
using Autofac;
using Autofac.Core.Resolving.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Sod.Infrastructure.Capabilities;

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
                .InstancePerDependency();

            builder
                .Register(ctx => new SerilogLoggerFactory(ctx.Resolve<Serilog.ILogger>()))
                .AsSelf()
                .InstancePerDependency();
        }
        
        public class LoggingMiddleware : IResolveMiddleware
        {
            public void Execute(ResolveRequestContext ctx, Action<ResolveRequestContext> next)
            {
                var instance = ctx.Instance;
                if (instance is LoggingCapability loggingCapability)
                {
                    loggingCapability.Logger = ctx.Resolve<SerilogLoggerFactory>().CreateLogger(instance.GetType());
                }
                
                next(ctx);
            }

            public PipelinePhase Phase => PipelinePhase.Activation;
        }
    }
}