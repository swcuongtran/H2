using H2.Application.Interfaces.Repositories;
using H2.Domain.Entities;
using H2.Infrastructure.Persistence.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace H2.Infrastructure.Persistence.Repositories
{
    public class SensorDataRepository : ISensorDataRepository
    {
        private readonly H2DbContext _context;
        public SensorDataRepository(H2DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(SensorData sensorData, CancellationToken cancellationToken)
        {
            await _context.SensorData.AddAsync(sensorData, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<SensorData>> GetAllAsync(string deviceId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
            }
            var query = await _context.SensorData
                .Where(sd => sd.DeviceId == deviceId)
                .OrderByDescending(sd => sd.Timestamp)
                .ToListAsync(cancellationToken);
            return query;
        }

        public async Task<IEnumerable<SensorData>> GetByDateRangeAsync(string deviceId, DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
            }
            var query = await _context.SensorData
                .Where(sd => sd.DeviceId == deviceId && sd.Timestamp >= from && sd.Timestamp <= to)
                .OrderBy(s => s.Timestamp)
                .ToListAsync(cancellationToken);
            return query;
        }

        public async Task<SensorData?> GetLatestAsync(string deviceId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
            }
            var query = await _context.SensorData
                .Where(sd => sd.DeviceId == deviceId)
                .OrderBy (s => s.Timestamp)
                .FirstOrDefaultAsync(cancellationToken);
            return query;
        }
    }
}
