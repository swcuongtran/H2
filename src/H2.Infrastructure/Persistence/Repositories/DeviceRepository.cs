using H2.Application.Interfaces.Repositories;
using H2.Domain.Entities;
using H2.Infrastructure.Persistence.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace H2.Infrastructure.Persistence.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly H2DbContext _context;
        public DeviceRepository(H2DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(Device device, CancellationToken cancellationToken)
        {
            await _context.Devices.AddAsync(device, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Device>> GetAllAsync(CancellationToken cancellationToken)
        {
            var query = await _context.Devices
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
            return query;
        }

        public async Task<Device> GetByIdAsync(string deviceId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
            }
            var query = await _context.Devices
                .Where(d => d.Id == deviceId)
                .FirstOrDefaultAsync(cancellationToken);
            if (query == null)
            {
                throw new KeyNotFoundException($"Device with ID {deviceId} not found.");
            }
            return query;
        }

        public async Task UpdateHeartbeatAsync(string deviceId, DateTime lastHeartbeat, CancellationToken cancellationToken)
        {
            var device = await GetByIdAsync(deviceId, cancellationToken);
            if (device is not null)
            {
                device.LastHeartbeat = lastHeartbeat;
                device.Status = Domain.Enums.DeviceStatus.Online;
                _context.Devices.Update(device);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
