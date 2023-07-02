using CompanyManagement.MessageIntegration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RabbitMQ.Client;
using System.Text;

namespace CompanyManagement.Tests.MessageIntegration.Tests
{
    public class PublisherHandlerTests
    {
        private readonly ServiceProvider _serviceProvider;

        public PublisherHandlerTests()
        {
            var services = new ServiceCollection();
            var mockedConnectionFactory = Substitute.For<IConnectionFactory>();
            services.AddSingleton(mockedConnectionFactory);
            services.AddSingleton<IPublisherHandler, PublisherHandler>();
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void Publish_Should_Be_Succesfull()
        {
            var message = "testfyhftjfuyj";
            var exchangeName = "test";
            var messageArray = Encoding.UTF8.GetBytes(message);

            var body = new ReadOnlyMemory<byte>(messageArray);
            var mockedConnectionFactory = _serviceProvider.GetRequiredService<IConnectionFactory>();
            var mockedConnection = Substitute.For<IConnection>();
            var mockedChannel = Substitute.For<IModel>();

            mockedConnectionFactory.CreateConnection().Returns(mockedConnection);
            mockedConnection.CreateModel().Returns(mockedChannel);
            var publisher = new PublisherHandler(mockedConnectionFactory);

            publisher.Publish(exchangeName, message);

            mockedChannel.Received(1).ExchangeDeclare(exchangeName, Arg.Is<string>(type => type == ExchangeType.Fanout));

            mockedChannel.Received(1).BasicPublish(exchangeName, "", null, Arg.Any<ReadOnlyMemory<byte>>());
        }

        [Fact]
        public void Publish_Should_Throw()
        {
            var message = "testfyhftjfuyj";
            var exchangeName = "test";
            var messageArray = Encoding.UTF8.GetBytes(message);

            var body = new ReadOnlyMemory<byte>(messageArray);
            var mockedConnectionFactory = _serviceProvider.GetRequiredService<IConnectionFactory>();
            var mockedConnection = Substitute.For<IConnection>();
            var mockedChannel = Substitute.For<IModel>();

            mockedConnectionFactory.CreateConnection().Returns(mockedConnection);
            mockedConnection.CreateModel().Returns(mockedChannel);
            var publisher = new PublisherHandler(mockedConnectionFactory);

            mockedChannel
                .When(x => x.ExchangeDeclare(exchangeName, Arg.Is<string>(type => type == ExchangeType.Fanout)))
                .Throw(new Exception());

            Assert.Throws<Exception>(() => publisher.Publish(exchangeName, message));

            mockedChannel.Received(1).ExchangeDeclare(exchangeName, Arg.Is<string>(type => type == ExchangeType.Fanout));
            mockedChannel.Received(0).BasicPublish(exchangeName, "", null, Arg.Any<ReadOnlyMemory<byte>>());
        }
    }
}