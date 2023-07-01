using HRManagementWebApi.Database;
using HRManagementWebApi.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagementWebApi.Service
{
    sealed public class EmployeeService : IEmployeeService
    {
        private readonly HRManagementDbContext _dbContext;

        public EmployeeService(HRManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            _dbContext.Employee.Add(employee);
            await _dbContext.SaveChangesAsync();

            return employee;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _dbContext.Employee.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _dbContext.Employee.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
