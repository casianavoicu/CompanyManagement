using RabbitMQ.Client;
using System.Text;

namespace CompanyManagement.MessageIntegration
{
    public class PublisherHandler : IPublisherHandler
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IConnectionFactory _connectionFactory;

        public PublisherHandler(IConnectionFactory connectionFactory)
        {
            //var factory = new ConnectionFactory { HostName = "localhost" };
            _connectionFactory = connectionFactory;
            _connectionFactory.UserName = "guest";
            _connectionFactory.Password = "guest";
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publish(string exchangeName, string message)
        {
            try
            {
                _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: exchangeName, "", null, body);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"message sent {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while publishing message: {ex.Message}");
                throw;
            }
        }
    }
}