namespace CompanyManagement.MessageIntegration
{
    public interface IQueueHandler 
    {
        void Register(string exchangeName, IMessageHandler message);
    }
}
