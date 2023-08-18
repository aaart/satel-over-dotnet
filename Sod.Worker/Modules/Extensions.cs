using Autofac;
using Microsoft.Extensions.Configuration;

namespace Sod.Worker.Modules;

public static class Extensions
{
    public static void RegisterConfiguration<T>(this ContainerBuilder builder, string section) where T : class
    {
        builder.Register(ctx => ctx.Resolve<IConfigurationRoot>().GetSection(section).Get<T>());
    }
}