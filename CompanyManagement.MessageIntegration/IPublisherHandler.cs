namespace CompanyManagement.MessageIntegration
{
    public interface IPublisherHandler
    {
        void Publish(string exchangeName, string message);
    }
}