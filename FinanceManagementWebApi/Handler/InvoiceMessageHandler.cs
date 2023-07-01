using CompanyManagement.Common.Dto;
using FinanceManagementWebApi.BusinessLogic;
using System.Text.Json;

namespace FinanceManagementWebApi.Handler
{
    sealed public class InvoiceMessageHandler : IInvoiceMessageHandler
    {
        private readonly IInvoiceBusinessLogic _invoiceBusinessLogic;

        public InvoiceMessageHandler(IInvoiceBusinessLogic invoiceBusinessLogic)
        {
            _invoiceBusinessLogic = invoiceBusinessLogic;
        }

        public void Process(string message)
        {
            var mappedInvoice = JsonSerializer.Deserialize<InvoiceDto>(message);

            Console.WriteLine($"Invoice Received from {mappedInvoice.Sender}");

            _invoiceBusinessLogic.CreateAsync(mappedInvoice).GetAwaiter().GetResult();

            Console.WriteLine("Invoice stored");
        }
    }
}
