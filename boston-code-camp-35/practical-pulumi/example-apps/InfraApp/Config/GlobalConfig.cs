using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pulumi;

using PurpleSpikeProductions.Iaac.ArcadeServices.Config;

namespace PulumiDemo.Config;

public record AzureConfig(string Location, string ResourceGroupName, string FunctionsPackagePath, string version);

public record GlobalConfig(
    AzureConfig AzureConfig,
    ExternalStacksInfoConfig ExternalStacksInfoConfig
    )
{
    public static async Task<GlobalConfig> LoadAsync(Pulumi.Config config)
    {
        await Task.CompletedTask;

        string location = config.Require("location");
        string version = config.Require("version");
        string resourceGroupName = config.Require("azure-resource-group-name");

        //Note: Releative path from inside the ~/example-apps/pulumi-demo directory
        string functionsPackagePath = config.Require("functions-package-path");

        var externalStacksConfig = config.RequireObject<ExternalStacksConfigDto>("external-stacks").GenerateValidConfigObject();
        var externalStacksInfoConfig = ExternalStacksInfoConfig.Load(externalStacksConfig);


        return new GlobalConfig(
            AzureConfig: new AzureConfig(location, resourceGroupName, functionsPackagePath, version),
            ExternalStacksInfoConfig: externalStacksInfoConfig
            );
    }
}

