namespace H2.Infrastructure.Cloud
{
    public class AwsS3Settings
    {
        public string BucketName { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
    }
}
