using Pulumi.AzureNative.Resources;

using Pulumi.AzureNative.AppConfiguration;
using Pulumi.AzureNative.AppConfiguration.Inputs;

using Pulumi;
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

    var appConfig = new ConfigurationStore("shared-app-config", new ConfigurationStoreArgs
    {
        ResourceGroupName = resourceGroup.Name,
        Location = resourceGroup.Location,
        CreateMode = CreateMode.Default,
        PublicNetworkAccess = PublicNetworkAccess.Enabled,
        EnablePurgeProtection = false,
        SoftDeleteRetentionInDays = 0,
        Identity = new ResourceIdentityArgs
        {
            Type = IdentityType.SystemAssigned
        },
        Sku = new SkuArgs
        {
            Name = "free"
        },
        DisableLocalAuth = false,
    });

    return new Dictionary<string, object?>
    {
        { "AzureSharedResourceGroupName", resourceGroup.Name },
        { "AppConfigResourceName", appConfig.Name }
    };
});

