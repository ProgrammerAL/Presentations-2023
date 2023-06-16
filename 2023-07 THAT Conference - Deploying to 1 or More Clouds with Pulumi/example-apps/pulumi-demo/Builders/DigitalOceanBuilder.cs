using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pulumi;
using Pulumi.DigitalOcean;

using PulumiDemo.Config;

namespace PulumiDemo.Builders;

public record DigitalOceanResources(SpacesBucket Bucket, string BucketServiceUrl);

public record DigitalOceanBuilder(GlobalConfig GlobalConfig)
{
    public DigitalOceanResources Build()
    {
        var spacesProvider = GenerateSpacesProvider("do-public-spaces-bucket-provider", GlobalConfig.DigitalOceanConfig.SpacesAccessId, GlobalConfig.DigitalOceanConfig.SpacesAccessSecret);

        var bucketRegion = Region.NYC3;
        var bucket = new SpacesBucket("public-data-bucket", new()
        {
            Region = bucketRegion,
            CorsRules = new[]
            {
                new Pulumi.DigitalOcean.Inputs.SpacesBucketCorsRuleArgs
                {
                    AllowedMethods = new[]
                    {
                        "GET",
                    },
                    AllowedOrigins = new[]
                    {
                        "*",
                    },
                    AllowedHeaders = new[]
                    {
                        "*"
                    }
                }
            },
        }, new CustomResourceOptions
        {
            Provider = spacesProvider
        });

        var bucketServiceUrl = $"https://{bucketRegion.ToString()}.digitaloceanspaces.com";

        return new DigitalOceanResources(bucket, bucketServiceUrl);
    }

    public Provider GenerateSpacesProvider(string name, string spacesAccessId, Output<string> spacesSecret)
    {
        return new Provider(name, new ProviderArgs
        {
            SpacesAccessId = (Input<string>)spacesAccessId,
            SpacesSecretKey = spacesSecret
        });
    }
}
