using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleFunctionApp;

public record PublicS3StorageConfig(string BucketEndpoint, string BucketName, string AccessId, string AccessSecret)
{
    public static PublicS3StorageConfig LoadFromConfig(IServiceProvider provider)
    {
        var bucketEndpoint = ConfigHelpers.LoadStringValueFromConfig(provider, nameof(PublicS3StorageConfig), nameof(BucketEndpoint));
        var bucketName = ConfigHelpers.LoadStringValueFromConfig(provider, nameof(PublicS3StorageConfig), nameof(BucketName));
        var accessId = ConfigHelpers.LoadStringValueFromConfig(provider, nameof(PublicS3StorageConfig), nameof(AccessId));
        var accessSecret = ConfigHelpers.LoadStringValueFromConfig(provider, nameof(PublicS3StorageConfig), nameof(AccessSecret));

        return new PublicS3StorageConfig(bucketEndpoint, bucketName, accessId, accessSecret);
    }
}

