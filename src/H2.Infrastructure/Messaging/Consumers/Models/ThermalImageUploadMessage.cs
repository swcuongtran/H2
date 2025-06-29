namespace H2.Infrastructure.Messaging.Consumers.Models
{
    public class ThermalImageUploadMessage
    {
        public string DeviceId { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public string Base64Image { get; set; } = default!;
        public string ContentType { get; set; } = "image/png";
        public DateTime CapturedAt { get; set; }
    }
}
