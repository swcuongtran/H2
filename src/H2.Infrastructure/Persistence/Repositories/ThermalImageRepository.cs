using H2.Application.Interfaces.Repositories;
using H2.Domain.Entities;
using H2.Infrastructure.Persistence.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace H2.Infrastructure.Persistence.Repositories
{
    public class ThermalImageRepository : IThermalImageRepository
    {
        private readonly H2DbContext _context;
        public ThermalImageRepository(H2DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(ThermalImage thermalImage, CancellationToken cancellationToken)
        {
            await _context.ThermalImages.AddAsync(thermalImage, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<ThermalImage>> GetByAnomalyAsync(bool IsAnomaly, CancellationToken cancellationToken)
        {
            var query = await _context.ThermalImages
                .Where(ti => ti.IsAnomaly == IsAnomaly)
                .OrderByDescending(ti => ti.CaptureAt)
                .ToListAsync(cancellationToken);
            return query;
        }

        public async Task<ThermalImage?> GetByIdAsync(Guid imageId, CancellationToken cancellationToken)
        {
            var query = await _context.ThermalImages
                .Where(ti => ti.Id == imageId)
                .FirstOrDefaultAsync(cancellationToken);
            return query ?? throw new KeyNotFoundException($"Thermal image with ID {imageId} not found.");
        }

        public async Task<IEnumerable<ThermalImage>> GetRecentAsync(int count, CancellationToken cancellationToken)
        {
            var query = await _context.ThermalImages
                .OrderByDescending(ti => ti.CaptureAt)
                .Take(count)
                .ToListAsync(cancellationToken);
            return query ?? throw new InvalidOperationException("No thermal images found.");
        }
    }
}
