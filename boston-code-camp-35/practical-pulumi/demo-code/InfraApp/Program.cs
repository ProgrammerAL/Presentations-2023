using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

using PulumiDemo;
using PulumiDemo.Builders;
using PulumiDemo.Config;

using System.Collections.Generic;

return await Pulumi.Deployment.RunAsync(async () =>
{
    var pulumiConfig = new Config();
    var globalConfig = await GlobalConfig.LoadAsync(pulumiConfig);

    var resourceGroup = new ResourceGroup(globalConfig.AzureConfig.ResourceGroupName, new ResourceGroupArgs
    { 
        Location = globalConfig.AzureConfig.Location
    });

    var azureBuilder = new AzureBuilder(globalConfig, resourceGroup);
    var azureResources = azureBuilder.Build();

    return new Dictionary<string, object?>
    {
        { "Readme", Output.Create(System.IO.File.ReadAllText("./Pulumi.README.md")) },
        { "FunctionHttpsEndpoint", azureResources.Function.HttpsEndpoint },
    };
});

