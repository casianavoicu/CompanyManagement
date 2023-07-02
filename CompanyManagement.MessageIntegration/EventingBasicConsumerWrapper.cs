using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.CodeAnalysis;

namespace CompanyManagement.MessageIntegration
{
    [ExcludeFromCodeCoverage]
    public class EventingBasicConsumerWrapper : IEventingBasicConsumerWrapper
    {
        public EventingBasicConsumer GetConsumer(IModel channel)
        {
            return new EventingBasicConsumer(channel);
        }
    }

}
