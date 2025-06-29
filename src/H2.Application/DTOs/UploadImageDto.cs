namespace H2.Application.DTOs
{
    public class UploadImageDto
    {
        public string DeviceId { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public Stream FileStream { get; set; } = default!;
        public string ContentType { get; set; } = "image/png";
        public DateTime CapturedAt { get; set; }
    }
}
