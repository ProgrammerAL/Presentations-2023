using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pulumi;

namespace PulumiDemo.Config;

public record AzureConfig(string Location, string ResourceGroupName);

public record GlobalConfig(
    AzureConfig AzureConfig
    )
{
    public static GlobalConfig Load(Pulumi.Config config)
    {
        string location = config.Require("location");
        string resourceGroupName = config.Require("azure-resource-group-name");

        return new GlobalConfig(
            AzureConfig: new AzureConfig(location, resourceGroupName)
            );
    }
}

