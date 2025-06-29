using H2.Application.Interfaces.Repositories;
using H2.Application.Interfaces.Services;
using H2.Domain.Entities;
using H2.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace H2.Infrastructure.Services
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly ILogger<AlertService> _logger;

        public AlertService(IAlertRepository alertRepository, ILogger<AlertService> logger)
        {
            _alertRepository = alertRepository ?? throw new ArgumentNullException(nameof(alertRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task SendAlertAsync(string deviceId, ArlertLevel arlertLevel, string message, DateTime timestamp, CancellationToken cancellationToken)
        {
            //logging
            _logger.LogWarning("[ALERT] Device: {DeviceId}, Level: {Level}, Msg: {Message}", deviceId, arlertLevel, message);
            //send sms

            //save db
            var alert = new Alert
            {
                Id = new Guid(),
                DeviceId = deviceId,
                Level = arlertLevel,
                Message = message,
                SentAt = timestamp,
            };

            await _alertRepository.AddAsync(alert, cancellationToken);
        }
    }
}
