using RabbitMQ.Client;
using System.Text;

namespace CompanyManagement.MessageIntegration
{
    public class PublisherHandler : IPublisherHandler
    {
        private IModel _channel;
        private IConnection _connection;

        public PublisherHandler()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            factory.UserName = "guest";
            factory.Password = "guest";
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publish(string exchangeName, string message)
        {
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: exchangeName, "", null, body);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"message sent {message}");
        }
    }
}
