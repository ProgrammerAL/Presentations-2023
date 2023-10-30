using System.Net;
using System.Text;

using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace ExampleFunctionApp
{
    public class CreatePublicFileFunction
    {
        private readonly PublicS3StorageConfig _publicS3StorageConfig;
        private readonly AmazonS3Client _client;

        public CreatePublicFileFunction(PublicS3StorageConfig publicS3StorageConfig)
        {
            _publicS3StorageConfig = publicS3StorageConfig;
            var credentials = new BasicAWSCredentials(_publicS3StorageConfig.AccessId, _publicS3StorageConfig.AccessSecret);
            var config = new AmazonS3Config();
            config.ServiceURL = _publicS3StorageConfig.BucketEndpoint;

            _client = new AmazonS3Client(credentials, config);
        }

        [Function("create-public-file")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var requestBody = await req.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(requestBody))
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                errorResponse.WriteString("Request body is null or empty");
                return errorResponse;
            }

            var blobBytes = Encoding.UTF8.GetBytes(requestBody);

            using var blobBinaryData = new MemoryStream();
            await blobBinaryData.WriteAsync(blobBytes);

            using var utility = new TransferUtility(_client);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = blobBinaryData,
                Key = $"public-files/{Guid.NewGuid()}",
                BucketName = _publicS3StorageConfig.BucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            await utility.UploadAsync(uploadRequest);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString($"Written to: {uploadRequest.Key}");

            return response;
        }
    }
}
