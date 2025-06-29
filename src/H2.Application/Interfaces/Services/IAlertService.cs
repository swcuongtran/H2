using H2.Domain.Enums;

namespace H2.Application.Interfaces.Services
{
    public interface IAlertService
    {
        Task SendAlertAsync(string deviceId, ArlertLevel arlertLevel, string message, DateTime timestamp, CancellationToken cancellationToken);
    }
}
