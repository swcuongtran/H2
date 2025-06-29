using H2.Application.Interfaces.Repositories;
using H2.Domain.Entities;
using H2.Infrastructure.Persistence.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace H2.Infrastructure.Persistence.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly H2DbContext _context;
        public AlertRepository(H2DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(Alert alert, CancellationToken cancellationToken)
        {
            await _context.Alerts.AddAsync(alert, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Alert>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var query = await _context.Alerts
                .Where(a => a.SentAt >= from && a.SentAt <= to)
                .OrderBy(a => a.SentAt)
                .ToListAsync(cancellationToken);
            return query;
        }

        public async Task<IEnumerable<Alert>> GetByDeviceAsync(string deviceId, CancellationToken cancellationToken)
        {
            var query = await _context.Alerts
                .Where(a => a.DeviceId == deviceId)
                .OrderByDescending(a => a.SentAt)
                .ToListAsync(cancellationToken);
            if (query == null || !query.Any())
            {
                throw new KeyNotFoundException($"No alerts found for device with ID {deviceId}.");
            }
            return query;
        }
    }
}
