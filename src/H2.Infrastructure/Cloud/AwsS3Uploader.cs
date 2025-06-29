using Amazon.S3;
using Amazon.S3.Transfer;
using H2.Application.Interfaces.Cloud;
using Microsoft.Extensions.Options;

namespace H2.Infrastructure.Cloud
{
    public class AwsS3Uploader : IS3Uploader
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AwsS3Settings _settings;
        public AwsS3Uploader(IAmazonS3 s3Client, IOptions<AwsS3Settings> options)
        {
            _s3Client = s3Client;
            _settings = options.Value; 
        }
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken)
        {
            var fileTransferUtility = new Amazon.S3.Transfer.TransferUtility(_s3Client);    
            var requests = new TransferUtilityUploadRequest
            {
                BucketName = _settings.BucketName,
                InputStream = fileStream,
                Key = fileName,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead 
            };

            await fileTransferUtility.UploadAsync(requests, cancellationToken);
            return $"https://{_settings.BucketName}.s3.{_settings.Region}.amazonaws.com/{fileName}";
        }
    }
}
