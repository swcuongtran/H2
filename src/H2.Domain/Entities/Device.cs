using H2.Domain.Enums;

namespace H2.Domain.Entities
{
    public class Device
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DeviceStatus Status { get; set; }

        public DateTime LastHeartbeat { get; set; }
        public string? Location { get; set; }
    }
}
