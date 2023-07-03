using RabbitMQ.Client;
using System.Text;

namespace CompanyManagement.MessageIntegration
{
    public class QueueHandler : IQueueHandler
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IEventingBasicConsumerWrapper _customEventingBasicConsumer;

        public QueueHandler(IConnectionFactory connectionFactory,
            IEventingBasicConsumerWrapper customEventingBasicConsumer)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.CreateConnection();

            _channel = _connection.CreateModel();
            _customEventingBasicConsumer = customEventingBasicConsumer;
        }

        public void Register(string exchangeName, IMessageHandler messageProcessor)
        {
            var receivedMessage = string.Empty;
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

            var queueDeclareResult = _channel.QueueDeclare();

            var queueName = queueDeclareResult.QueueName;

            var consumer = _customEventingBasicConsumer.GetConsumer(_channel);
            _channel.QueueBind(queueName, exchangeName, "");
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                receivedMessage = message;
                Console.WriteLine(message);
                messageProcessor.Process(message);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}