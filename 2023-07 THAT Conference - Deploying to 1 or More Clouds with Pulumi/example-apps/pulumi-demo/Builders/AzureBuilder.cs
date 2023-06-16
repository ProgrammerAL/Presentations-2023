using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pulumi;
using Pulumi.AzureNative;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

using PulumiDemo.Config;

using AzureNative = Pulumi.AzureNative;

namespace PulumiDemo.Builders;

public record AzureResources(AzureResources.ServiceStorageInfra ServiceStorage, AzureResources.FunctionInfra Function)
{
    public record ServiceStorageInfra(StorageAccount StorageAccount, BlobContainer FunctionsContainer, Blob FunctionsBlob);
    public record FunctionInfra(WebApp WebApp, Output<string> HttpsEndpoint);
}

public record AzureBuilder(
    GlobalConfig GlobalConfig, 
    ResourceGroup ResourceGroup, 
    DigitalOceanResources DigitalOceanResources)
{
    public AzureResources Build()
    {
        var storageInfra = GenerateStorageInfrastructure();
        var functionsInfra = GenerateFunctionsInfrastructure(storageInfra);

        return new AzureResources(storageInfra, functionsInfra);
    }

    private AzureResources.ServiceStorageInfra GenerateStorageInfrastructure()
    {
        var storageAccount = new StorageAccount("funcs-storage", new StorageAccountArgs
        {
            ResourceGroupName = ResourceGroup.Name,
            Sku = new AzureNative.Storage.Inputs.SkuArgs
            {
                Name = AzureNative.Storage.SkuName.Standard_GRS,
            },
            Kind = Kind.StorageV2,
            EnableHttpsTrafficOnly = true,
            MinimumTlsVersion = MinimumTlsVersion.TLS1_2,
            AccessTier = AccessTier.Hot,
            AllowSharedKeyAccess = true,
            SasPolicy = new AzureNative.Storage.Inputs.SasPolicyArgs
            {
                ExpirationAction = ExpirationAction.Log,
                SasExpirationPeriod = "00.01:00:00"
            }
        });

        //Storage Container to host the Azure Functions
        var functionsContainer = new BlobContainer("functions-container", new BlobContainerArgs
        {
            AccountName = storageAccount.Name,
            PublicAccess = PublicAccess.None,
            ResourceGroupName = ResourceGroup.Name,
        });

        var functionsBlob = new Blob("functions-blob", new BlobArgs
        {
            AccountName = storageAccount.Name,
            ContainerName = functionsContainer.Name,
            AccessTier = BlobAccessTier.Hot,
            ResourceGroupName = ResourceGroup.Name,
            Source = new FileArchive(GlobalConfig.AzureConfig.FunctionsPackagePath)
        });

        return new AzureResources.ServiceStorageInfra(
            storageAccount,
            functionsContainer,
            functionsBlob);
    }

    private AzureResources.FunctionInfra GenerateFunctionsInfrastructure(AzureResources.ServiceStorageInfra storageInfra)
    {
        //Create the App Service Plan
        var appServicePlan = new AppServicePlan("functions-app-service-plan", new AppServicePlanArgs
        {
            ResourceGroupName = ResourceGroup.Name,
            Kind = "Linux",
            Sku = new SkuDescriptionArgs
            {
                Tier = "Dynamic",
                Name = "Y1"
            },
            // For Linux, you need to change the plan to have Reserved = true property.
            Reserved = true,
        });

        var functionAppSiteConfig = new SiteConfigArgs
        {
            LinuxFxVersion = "DOTNET-ISOLATED|7.0",
            AppSettings = new[]
            {
                new NameValuePairArgs{
                    Name = "AzureWebJobsStorage__accountname",
                    Value = storageInfra.StorageAccount.Name
                },
                new NameValuePairArgs{
                    Name = "WEBSITE_RUN_FROM_PACKAGE",
                    Value = storageInfra.FunctionsBlob.Url
                },
                new NameValuePairArgs
                {
                    Name = "FUNCTIONS_WORKER_RUNTIME",
                    Value = "dotnet-isolated",
                },
                new NameValuePairArgs
                {
                    Name = "FUNCTIONS_EXTENSION_VERSION",
                    Value = "~4",
                },
                new NameValuePairArgs
                {
                    Name = "SCM_DO_BUILD_DURING_DEPLOYMENT",
                    Value = "0"
                },
                new NameValuePairArgs
                {
                    Name = "PublicS3StorageConfig__BucketEndpoint",
                    Value = DigitalOceanResources.BucketServiceUrl,
                },
                new NameValuePairArgs
                {
                    Name = "PublicS3StorageConfig__BucketName",
                    Value = DigitalOceanResources.Bucket.Name,
                },
                new NameValuePairArgs
                {
                    Name = "PublicS3StorageConfig__AccessId",
                    Value = GlobalConfig.DigitalOceanConfig.SpacesAccessId
                },
                new NameValuePairArgs
                {
                    Name = "PublicS3StorageConfig__AccessSecret",
                    Value = GlobalConfig.DigitalOceanConfig.SpacesAccessSecret
                },
            }
        };

        //Create the App Service
        var webApp = new WebApp("functions-app", new WebAppArgs
        {
            Kind = "FunctionApp",
            ResourceGroupName = ResourceGroup.Name,
            ServerFarmId = appServicePlan.Id,
            HttpsOnly = true,
            SiteConfig = functionAppSiteConfig,
            ClientAffinityEnabled = false,
            Identity = new ManagedServiceIdentityArgs
            {
                Type = AzureNative.Web.ManagedServiceIdentityType.SystemAssigned,
            },
        });

        var httpsEndpoint = webApp.DefaultHostName.Apply(x => $"https://{x}");

        return new AzureResources.FunctionInfra(
            webApp,
            httpsEndpoint);
    }
}
