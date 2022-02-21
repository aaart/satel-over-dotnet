using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Sod.Worker.Modules
{
    public static class Extensions
    {
        public static void RegisterOptions<T>(this ContainerBuilder builder) where T : class
            => builder.Register(
                ctx =>
                    Options.Create(
                        ctx.Resolve<IConfigurationRoot>().GetSection(typeof(T).Name.Replace("Options", String.Empty)).Get<T>()));
    }
}