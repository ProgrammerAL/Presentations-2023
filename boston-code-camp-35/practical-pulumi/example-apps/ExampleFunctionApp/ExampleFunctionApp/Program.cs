using Microsoft.Extensions.Configuration;
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

            options.Connect(Environment.GetEnvironmentVariable("AppConfigConnectionString"))
                                   .Select($"ExampleFunctionApp:{envrionment}")
                                   .ConfigureRefresh(refreshOptions =>
                                       refreshOptions.Register($"ExampleFunctionApp:{envrionment}:Sentinel", refreshAll: true));
        });
    })
    .ConfigureServices(serviceCollection =>
    {
        //serviceCollection.AddSingleton(x => ServiceConfig.LoadFromConfig(x));
        serviceCollection.AddAzureAppConfiguration();
    })
    .ConfigureFunctionsWorkerDefaults(x =>
    {
        x.UseAzureAppConfiguration();
    })
    .Build();

host.Run();
