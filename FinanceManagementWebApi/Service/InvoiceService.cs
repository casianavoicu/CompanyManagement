using FinanceManagementWebApi.Database;
using FinanceManagementWebApi.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagementWebApi.Service
{
    public class InvoiceService : IInvoiceService
    {
        private readonly FinanceManagementDbContext _context;

        public InvoiceService(FinanceManagementDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Invoice invoice)
        {
            _context.Invoice.Add(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Invoice>> GetAllAsync()
        {
            return await _context.Invoice.ToListAsync();
        }
    }
}
