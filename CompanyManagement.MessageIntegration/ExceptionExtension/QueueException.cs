namespace CompanyManagement.MessageIntegration.ExceptionExtension
{
    public class QueueException : Exception
    {
        public QueueException() : base() { }

        public QueueException(string message) : base(message) { }

        public QueueException(string message, Exception innerException) : base(message, innerException) { }
    }
}
