using H2.Domain.Enums;

namespace H2.Domain.Entities
{
    public class ThermalImage
    {
        public Guid Id { get; set; }
        public string DeviceId { get; set; } = null!;
        public DateTime CaptureAt { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public ImageProcessStatus ProcessStatus { get; set; }
        public double Confident { get; set; }
        public bool IsAnomaly { get; set; }
    }
}
