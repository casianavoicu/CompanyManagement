namespace CompanyManagement.MessageIntegration
{
    public interface IQueueHandler 
    {
        void Register(string exchangeName, string to, IMessageHandler message);
    }
}
