using CompanyManagement.Common.Dto;
using FinanceManagementWebApi.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagementWebApi.BusinessLogic
{
    public interface IInvoiceBusinessLogic
    {
        Task CreateAsync(InvoiceDto invoice);

        Task<ActionResult<List<Invoice>>> GetAllAsync();
    }
}
