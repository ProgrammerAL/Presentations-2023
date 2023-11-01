using Pulumi;
using Pulumi.AzureNative.AppConfiguration;
using Pulumi.AzureNative.Resources;

using System;
using System.Threading.Tasks;

namespace PurpleSpikeProductions.Iaac.ArcadeServices.Config;
public record ExternalStacksInfoConfig(
    Output<GetResourceGroupResult> SharedResourceGroup,
    Output<GetConfigurationStoreResult> SharedAppConfig)
{
    public static ExternalStacksInfoConfig Load(ExternalStacksConfig externalStacksConfig)
    {
        var sharedStack = new StackReference(externalStacksConfig.SharedStackName);

        var resourceGroupNameOutput = LoadOutputValue(sharedStack, "AzureSharedResourceGroupName");
        var appConfigResourceNameOutput = LoadOutputValue(sharedStack, "AppConfigResourceName");

        var resourceGroup = GetResourceGroup.Invoke(new GetResourceGroupInvokeArgs
        {
            ResourceGroupName = resourceGroupNameOutput
        });

        var appConfig = GetConfigurationStore.Invoke(new GetConfigurationStoreInvokeArgs
        {
            ResourceGroupName = resourceGroupNameOutput,
            ConfigStoreName = appConfigResourceNameOutput
        });

        return new ExternalStacksInfoConfig(
            resourceGroup, appConfig);
    }

    private static Output<string> LoadOutputValue(StackReference stackRef, string name)
    {
        var value = stackRef.RequireOutput(name);
        return value.Apply(x => x?.ToString()!)!;
    }
}
