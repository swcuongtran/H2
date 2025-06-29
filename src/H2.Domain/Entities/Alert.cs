using H2.Domain.Enums;

namespace H2.Domain.Entities
{
    public class Alert
    {
        public Guid Id { get; set; }
        public string DeviceId { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public ArlertLevel Level { get; set; }
        public string? Message { get; set; }
        public NotificationChannel Channel { get; set; }
    }
}
