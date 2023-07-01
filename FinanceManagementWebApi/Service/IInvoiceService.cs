using FinanceManagementWebApi.Database.Entities;

namespace FinanceManagementWebApi.Service
{
    public interface IInvoiceService 
    {
        Task CreateAsync(Invoice invoice);

        Task<List<Invoice>> GetAllAsync();
    }
}
