namespace Jwt.Services.RabbitMq
{
    public interface IMessagePublisher
    {
        public void PublishMessage(string exchangeName, string routingKey, string queueName, string message);
    }
}
