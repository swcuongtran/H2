using H2.Application.Interfaces.Cloud;
using H2.Application.Interfaces.Repositories;
using H2.Domain.Entities;
using H2.Infrastructure.Messaging;
using H2.Infrastructure.Messaging.Consumers.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace H2.Infrastructure.Interfaces.Messaging.Consumers
{
    public class UploadToS3Consumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string QueueName = "upload-to-s3";

        public UploadToS3Consumer(IServiceProvider serviceProvider, RabbitMqSettings rabbitMqSettings)
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
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (sender, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var message = JsonSerializer.Deserialize<ThermalImageUploadMessage>(json);
                    if (message == null)
                    {
                        throw new InvalidOperationException("Deserialized message is null.");
                    }
                    using var scope = _serviceProvider.CreateScope();
                    var uploader = scope.ServiceProvider.GetRequiredService<IS3Uploader>();
                    var repository = scope.ServiceProvider.GetRequiredService<IThermalImageRepository>();

                    // Convert base 64 to Stream
                    byte[] imageBytes = Convert.FromBase64String(message.Base64Image);
                    await using var stream = new MemoryStream(imageBytes);

                    //Upload to S3
                    var imageUrl = await uploader.UploadFileAsync(stream, message.FileName, message.ContentType, stoppingToken);

                    //Create ThermalImage entity
                    var image = new ThermalImage
                    {
                        Id = Guid.NewGuid(),
                        DeviceId = message.DeviceId,
                        CaptureAt = message.CapturedAt,
                        ImageUrl = imageUrl,
                        FileName = message.FileName,
                        ProcessStatus = Domain.Enums.ImageProcessStatus.Pending,
                        Confident = 0.0,
                        IsAnomaly = false
                    };
                    //Save to database
                    await repository.AddAsync(image, stoppingToken);

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");

                }
            };
            _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }
        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
