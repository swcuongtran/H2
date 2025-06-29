using H2.Domain.Entities;

namespace H2.Application.Interfaces.Repositories
{
    public interface IAlertRepository
    {
        Task AddAsync(Alert alert, CancellationToken cancellationToken);
        Task<IEnumerable<Alert>> GetByDeviceAsync(string deviceId, CancellationToken cancellationToken);
        Task<IEnumerable<Alert>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
    }
}
