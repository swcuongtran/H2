namespace H2.Infrastructure.Messaging
{
    public class RabbitMqSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
