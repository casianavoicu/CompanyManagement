using FinanceManagementWebApi.Database;
using FinanceManagementWebApi.Database.Entities;

namespace FinanceManagementWebApi.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly FinanceManagementDbContext _context;

        public EmployeeService(FinanceManagementDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Employee employee)
        {
            _context.Employee.Add(employee);

            await _context.SaveChangesAsync();
        }
    }
}