using H2.Domain.Entities;

namespace H2.Application.Interfaces.Repositories
{
    public interface IDeviceRepository
    {
        Task<Device> GetByIdAsync(string deviceId, CancellationToken cancellationToken);
        Task<IEnumerable<Device>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Device device, CancellationToken cancellationToken);
        Task UpdateHeartbeatAsync(string deviceId, DateTime lastHeartbeat, CancellationToken cancellationToken);
    }
}
