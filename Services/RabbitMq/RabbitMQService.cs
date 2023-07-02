using RabbitMQ.Client;
using System.Text;

namespace Jwt.Services.RabbitMq
{
    public class RabbitMQService : IMessagePublisher
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;

        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;
            CreateConnection();
        }

        private void CreateConnection()
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri(_configuration.GetSection("RabbitMQ:Uri").Value);
            factory.ClientProvidedName = _configuration.GetSection("RabbitMQ:ClientProvidedName").Value;

            _connection = factory.CreateConnection();
        }

        public void PublishMessage(string exchangeName, string routingKey, string queueName, string message)
        {
            using (IModel channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queueName, exchangeName, routingKey, null);

                byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
            }
        }
    }
}
