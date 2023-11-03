using ExampleFunctionApp;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration(x =>
    {
#if DEBUG
        x.AddJsonFile("host.json");
        x.AddJsonFile("local.settings.json");
#endif
    })
    .ConfigureServices(serviceCollection =>
    {
        serviceCollection.AddOptions<ServiceConfig>()
        .Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection(nameof(ServiceConfig)).Bind(settings);
        });
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
