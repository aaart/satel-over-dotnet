using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sod.Worker.Modules;

namespace Sod.Worker;

// ReSharper disable once ClassNeverInstantiated.Global
public class Program
{
    public static void Main(string[] args)
    {
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        CreateHostBuilder(args).Build().Run();
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(
                new AutofacServiceProviderFactory(
                    builder =>
                        builder
                            // Order DOES matter, otherwise logging does not work 
                            .RegisterModule<ConfigurationModule>()
                            .RegisterModule<LoggingModule>()
                            .RegisterModule<InfrastructureModule>()))
            .ConfigureServices((_, services) => services.AddHostedService<Worker>());
    }
}