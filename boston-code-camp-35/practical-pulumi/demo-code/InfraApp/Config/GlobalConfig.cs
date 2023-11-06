using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pulumi;

namespace PulumiDemo.Config;

public record GlobalConfig(
    ServiceConfig ServiceConfig,
    AzureConfig AzureConfig,
    ExternalStacksInfoConfig ExternalStacksInfoConfig
    )
{
    public static async Task<GlobalConfig> LoadAsync(Pulumi.Config config)
    {
        var azureClientConfig = await Pulumi.AzureNative.Authorization.GetClientConfig.InvokeAsync();

        var azureConfig = new AzureConfigDto
        {
            ClientConfig = azureClientConfig,
            Location = config.Require("location"),
            ResourceGroupName = config.Require("azure-resource-group-name"),
            FunctionsPackagePath = config.Require("functions-package-path")
        };

        var externalStacksConfig = config.RequireObject<ExternalStacksConfigDto>("external-stacks").GenerateValidConfigObject();
        var externalStacksInfoConfig = ExternalStacksInfoConfig.Load(externalStacksConfig);

        return new GlobalConfig(
            ServiceConfig: config.RequireObject<ServiceConfigDto>("service-config").GenerateValidConfigObject(),
            AzureConfig: azureConfig.GenerateValidConfigObject(),
            ExternalStacksInfoConfig: externalStacksInfoConfig
            );
    }
}

