namespace H2.Application.Interfaces.Cloud
{
    public interface IS3Uploader
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken);
    }
}
