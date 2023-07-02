using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CompanyManagement.MessageIntegration
{
    public interface IEventingBasicConsumerWrapper
    {
        EventingBasicConsumer GetConsumer(IModel channel);
    }
}