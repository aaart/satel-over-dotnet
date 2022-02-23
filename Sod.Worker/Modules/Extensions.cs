using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Sod.Worker.Modules
{
    public static class Extensions
    {
        public static void RegisterOptions<T>(this ContainerBuilder builder, string section = null) where T : class
        {
            section ??= typeof(T).Name.Replace("Options", String.Empty);
            builder.Register(
                ctx =>
                    Options.Create(
                        ctx.Resolve<IConfigurationRoot>().GetSection(section).Get<T>()));
        }
    }
}