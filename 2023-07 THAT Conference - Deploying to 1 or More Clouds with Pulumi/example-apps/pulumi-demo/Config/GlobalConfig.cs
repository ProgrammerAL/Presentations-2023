using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pulumi;

namespace PulumiDemo.Config;

public record AzureConfig(string Location, string ResourceGroupName, string FunctionsPackagePath);
public record DigitalOceanConfig(
    Output<string> Token,
    string SpacesAccessId,
    Output<string> SpacesAccessSecret);

public record GlobalConfig(
    AzureConfig AzureConfig,
    DigitalOceanConfig DigitalOceanConfig
    )
{
    public static GlobalConfig Load(Pulumi.Config config)
    {
        string location = config.Require("location");
        string resourceGroupName = config.Require("azure-resource-group-name");
        string functionsPackagePath = config.Require("functions-package-path");

        var doToken = config.RequireSecret("do-token");
        var doSpacesAccessId = config.Require("do-spackes-access-id");
        var doSpacesAccessSecret = config.RequireSecret("do-spaces-access-secret");

        return new GlobalConfig(
            AzureConfig: new AzureConfig(location, resourceGroupName, functionsPackagePath),
            DigitalOceanConfig: new DigitalOceanConfig(doToken, doSpacesAccessId, doSpacesAccessSecret)
            );
    }
}

