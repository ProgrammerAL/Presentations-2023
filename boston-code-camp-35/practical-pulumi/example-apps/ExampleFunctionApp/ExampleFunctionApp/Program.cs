using ExampleFunctionApp;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration(x =>
    {
#if DEBUG
        x.AddJsonFile("host.json");
        x.AddJsonFile("local.settings.json");
#endif

        x.AddAzureAppConfiguration(options =>
        {
            var connectionString = Environment.GetEnvironmentVariable("AppConfigConnectionString") ?? throw new Exception("Missing environment variable AppConfigConnectionString");
            var envrionment = Environment.GetEnvironmentVariable("AppConfigEnvironment") ?? throw new Exception("Missing environment variable AppConfigConnectionString");

            options.Connect(connectionString).Select(KeyFilter.Any, envrionment);
        });

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
