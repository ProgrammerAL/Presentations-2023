using Pulumi;
using Pulumi.AzureNative.Authorization;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraApp.Config;
public record AzureConfig(
    GetClientConfigResult ClientConfig,
    string Location,
    string ResourceGroupName,
    string FunctionsPackagePath);

public class AzureConfigDto : ConfigDtoBase<AzureConfig>
{
    public GetClientConfigResult? ClientConfig { get; set; }
    public string? Location { get; set; }
    public string? ResourceGroupName { get; set; }
    public string? FunctionsPackagePath { get; set; }

    public override AzureConfig GenerateValidConfigObject()
    {
        if (ClientConfig != null
            && !string.IsNullOrWhiteSpace(Location)
            && !string.IsNullOrWhiteSpace(ResourceGroupName)
            && !string.IsNullOrWhiteSpace(FunctionsPackagePath))
        {
            return new(ClientConfig, Location, ResourceGroupName, FunctionsPackagePath);
        }

        throw new Exception($"{GetType().Name} has invalid config");
    }
}
