namespace H2.Infrastructure.Messaging.Consumers.Models
{
    public class SensorDataMessage
    {
        public string DeviceId { get; set; } = null!;
        public double Ppm { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
