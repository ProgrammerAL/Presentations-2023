using System.Net;
using System.Text;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ExampleFunctionApp.Functions;

public class HealthCheckFunction
{
    private readonly IOptions<ServiceConfig> _serviceConfig;

    public HealthCheckFunction(IOptions<ServiceConfig> serviceConfig, IConfiguration config)
    {
        _serviceConfig = serviceConfig;
    }

    [Function("health-check")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        var responseObject = new Response(Version: _serviceConfig.Value.Version, Environment: _serviceConfig.Value.Environment);
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(responseObject);

        return response;
    }

    private record Response(string Version, string Environment);
}
