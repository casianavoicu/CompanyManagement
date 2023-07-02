using CompanyManagement.MessageIntegration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CompanyManagement.Tests.MessageIntegration.Tests
{
    public class QueueHandlerTests
    {
        private readonly ServiceProvider _serviceProvider;

        public QueueHandlerTests()
        {
            var services = new ServiceCollection();
            var mockedConnectionFactory = Substitute.For<IConnectionFactory>();
            var mockedMessageHandler = Substitute.For<IMessageHandler>();
            var mockedCustomEventingBasicConsumer = Substitute.For<IEventingBasicConsumerWrapper>();
            services.AddSingleton(mockedConnectionFactory);
            services.AddSingleton(mockedMessageHandler);
            services.AddSingleton(mockedCustomEventingBasicConsumer);
            services.AddSingleton<IQueueHandler, QueueHandler>();
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void Register_Should_Call_Consume_When_Called()
        {
            var queueName = "test";
            var exchangeName = "test";

            var mockedMessageHandler = _serviceProvider.GetRequiredService<IMessageHandler>();
            var mockedConnectionFactory = _serviceProvider.GetRequiredService<IConnectionFactory>();
            var mockedEventingBasicConsumerMock = Substitute.For<IEventingBasicConsumerWrapper>();
            var mockedConnection = Substitute.For<IConnection>();
            var mockedChannel = Substitute.For<IModel>();

            var eventing = new EventingBasicConsumer(mockedChannel);

            mockedConnectionFactory.CreateConnection().Returns(mockedConnection);
            mockedConnection.CreateModel().Returns(mockedChannel);
            mockedChannel.QueueDeclare().Returns(new QueueDeclareOk(queueName, 0, 0));
            mockedEventingBasicConsumerMock.GetConsumer(mockedChannel).Returns(eventing);

            var queueHandler = new QueueHandler(mockedConnectionFactory, mockedEventingBasicConsumerMock);
            queueHandler.Register(exchangeName, mockedMessageHandler);

            mockedChannel.Received(1).ExchangeDeclare(Arg.Any<string>(), ExchangeType.Fanout);

            mockedChannel.Received(1).QueueBind(queueName, Arg.Any<string>(), Arg.Any<string>());

            mockedChannel.Received(1).BasicConsume(queue: queueName, autoAck: true, consumer: Arg.Any<IBasicConsumer>());
        }
    }
}