using H2.Application.Interfaces.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace H2.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IMessageQueue
    {
        private readonly RabbitMqSettings _settings;
        private readonly IConnection _connection;

        public RabbitMqPublisher(IOptions<RabbitMqSettings> options, CancellationToken cancellationToken)
        {
            _settings = options.Value;
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password
            };
            _connection = factory.CreateConnection();
        }

        public Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken)
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            return Task.CompletedTask;
        }
    }
}
