namespace H2.Application.Interfaces.Messaging
{
    public interface IMessageQueue
    {
        Task PublishAsync<T>(string queueName,T message, CancellationToken cancellationToken);
    }
}
