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

return await Pulumi.Deployment.RunAsync(() =>
{
    var pulumiConfig = new Config();
    var globalConfig = GlobalConfig.Load(pulumiConfig);

    var resourceGroup = new ResourceGroup(globalConfig.AzureConfig.ResourceGroupName, new ResourceGroupArgs
    { 
        Location = globalConfig.AzureConfig.Location
    });

    var doBuilder = new DigitalOceanBuilder(globalConfig);
    var doResources = doBuilder.Build();

    var azureBuilder = new AzureBuilder(globalConfig, resourceGroup, doResources);
    var azureResources = azureBuilder.Build();

    return new Dictionary<string, object?>
    {
        { "PublicStorageEndpoint", doResources.BucketServiceUrl },
        { "FunctionHttpsEndpoint", azureResources.Function.HttpsEndpoint },
        { "Readme", Output.Create(System.IO.File.ReadAllText("./Pulumi.README.md"))}
    };
});

