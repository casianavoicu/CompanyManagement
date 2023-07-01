using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CompanyManagement.MessageIntegration
{
    public class QueueHandler : IQueueHandler
    {
        private IModel _channel;
        private IConnection _connection;

        public QueueHandler()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            factory.UserName = "guest";
            factory.Password = "guest";
            _connection = factory.CreateConnection();
          
            _channel = _connection.CreateModel();
        }
      
        public void Register(string exchangeName, string to, IMessageHandler messageProcessor)
        {
            var receivedMessage = string.Empty;
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;

            var consumer = new EventingBasicConsumer(_channel);
            _channel.QueueBind(queueName, exchangeName, "");
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                receivedMessage = message;
                Console.WriteLine(message + " " + to);
                messageProcessor.Process(message);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        }
    }
}
