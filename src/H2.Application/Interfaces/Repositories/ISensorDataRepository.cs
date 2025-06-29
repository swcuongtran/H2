using H2.Domain.Entities;

namespace H2.Application.Interfaces.Repositories
{
    public interface ISensorDataRepository
    {
        Task AddAsync (SensorData sensorData, CancellationToken cancellationToken);
        Task<SensorData?> GetLatestAsync(string deviceId, CancellationToken cancellationToken);
        Task<IEnumerable<SensorData>> GetByDateRangeAsync(string deviceId,DateTime from, DateTime to, CancellationToken cancellationToken);
        Task<IEnumerable<SensorData>> GetAllAsync(string deviceId, CancellationToken cancellationToken);
    }
}
