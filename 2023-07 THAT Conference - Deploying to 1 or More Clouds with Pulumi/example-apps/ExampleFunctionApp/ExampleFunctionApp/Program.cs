using ExampleFunctionApp;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
#if DEBUG
            .ConfigureAppConfiguration(x =>
            {
                x.AddJsonFile("host.json");
                x.AddJsonFile("local.settings.json");
            })
#endif
    .ConfigureServices(serviceCollection =>
    {
        serviceCollection.AddSingleton(x => PublicS3StorageConfig.LoadFromConfig(x));
    })
    .Build();

host.Run();
