using CompanyManagement.Common.Dto;
using FinanceManagementWebApi.Database.Entities;
using FinanceManagementWebApi.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FinanceManagementWebApi.BusinessLogic
{
    public class InvoiceBusinessLogic : IInvoiceBusinessLogic
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceBusinessLogic(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public async Task CreateAsync(InvoiceDto invoice)
        {
            if (invoice.Product == null)
                throw new ArgumentNullException(nameof(invoice.Product));

            var dbInvoice = new Invoice
            {
                InvoiceBody = JsonSerializer.Serialize(invoice),
            };

            await _invoiceService.CreateAsync(dbInvoice);
        }

        public async Task<ActionResult<List<Invoice>>> GetAllAsync()
        {
            return await _invoiceService.GetAllAsync();
        }
    }
}