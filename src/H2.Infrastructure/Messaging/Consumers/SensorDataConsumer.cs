using H2.Application.Interfaces.Repositories;
using H2.Application.Interfaces.Services;
using H2.Domain.Entities;
using H2.Domain.Enums;
using H2.Infrastructure.Messaging.Consumers.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace H2.Infrastructure.Messaging.Consumers
{
    public class SensorDataConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string QueueName = "sensor-data";

        public SensorDataConsumer(IServiceProvider serviceProvider, RabbitMqSettings rabbitMqSettings)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqSettings.Host,
                UserName = rabbitMqSettings.Username,
                Password = rabbitMqSettings.Password,
                Port = rabbitMqSettings.Port
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false);
        }
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var message = JsonSerializer.Deserialize<SensorDataMessage>(json);
                    if (message == null)
                    {
                        throw new InvalidOperationException("Deserialized message is null.");
                    }
                    using var scope = _serviceProvider.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<ISensorDataRepository>();
                    var alertService = scope.ServiceProvider.GetRequiredService<IAlertService>();
                    var hub = scope.ServiceProvider.GetRequiredService<IHubNotifier>();

                    var alertLevel = ArlertLevel.None;
                    if (message.Ppm >= 500) alertLevel = ArlertLevel.Critical;
                    else if (message.Ppm >= 300) alertLevel = ArlertLevel.Warning;
                    else if (message.Ppm >= 100) alertLevel = ArlertLevel.Unsafe;

                    var data = new SensorData
                    {
                        Id = Guid.NewGuid(),
                        DeviceId = message.DeviceId,
                        Timestamp = message.Timestamp,
                        Ppm = message.Ppm,
                        AlertLevel = alertLevel
                    };
                    await repository.AddAsync(data, cancellationToken);

                    await hub.PushSensorDataAsync(data, cancellationToken);

                    if (alertLevel != ArlertLevel.None)
                    {
                        await alertService.SendAlertAsync(
                            deviceId: data.DeviceId,
                            arlertLevel: alertLevel,
                            message: $"H₂ level = {data.Ppm} ppm — {alertLevel}",
                            timestamp: data.Timestamp,
                            cancellationToken: cancellationToken
                        );
                    }
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, true); // Requeue the message
                }
            };
            _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
