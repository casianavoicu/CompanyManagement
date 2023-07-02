namespace CompanyManagement.MessageIntegration
{
    public interface IMessageHandler
    {
        void Process(string message);
    }
}