using H2.Domain.Entities;

namespace H2.Application.Interfaces.Repositories
{
    public interface IThermalImageRepository
    {
        Task AddAsync(ThermalImage thermalImage, CancellationToken cancellationToken);
        Task<ThermalImage?> GetByIdAsync(Guid imageId, CancellationToken cancellationToken);
        Task<IEnumerable<ThermalImage>> GetRecentAsync(int count, CancellationToken cancellationToken);
        Task<IEnumerable<ThermalImage>> GetByAnomalyAsync(bool IsAnomaly, CancellationToken cancellationToken);
    }
}
