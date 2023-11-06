using Pulumi;
using Pulumi.AzureNative.AppConfiguration;
using Pulumi.AzureNative.AppConfiguration.Outputs;
using Pulumi.AzureNative.Resources;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace PulumiDemo.Config;
public record ExternalStacksInfoConfig(
    Output<GetResourceGroupResult> SharedResourceGroup,
    Output<GetConfigurationStoreResult> SharedAppConfig,
    Output<ApiKeyResponse> AccessKey)
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

        var configStoreKeys = ListConfigurationStoreKeys.Invoke(new ListConfigurationStoreKeysInvokeArgs
        {
            ResourceGroupName = resourceGroupNameOutput,
            ConfigStoreName = appConfigResourceNameOutput,
        });

        var accessKey = configStoreKeys.Apply(x => x.Value.First());

        return new ExternalStacksInfoConfig(
            resourceGroup, appConfig, accessKey);
    }

    private static Output<string> LoadOutputValue(StackReference stackRef, string name)
    {
        var value = stackRef.RequireOutput(name);
        return value.Apply(x => x?.ToString()!)!;
    }
}
