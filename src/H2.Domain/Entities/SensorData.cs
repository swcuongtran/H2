using H2.Domain.Enums;

namespace H2.Domain.Entities
{
    public class SensorData
    {
        public Guid Id { get; set; }
        public string DeviceId { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public double PPM { get; set; }
        public ArlertLevel AlertLevel { get; set; }
    }
}
