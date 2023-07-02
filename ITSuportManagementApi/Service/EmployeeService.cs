using ITSuportManagementApi.Database;
using ITSuportManagementApi.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITSuportManagementApi.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ITSupportManagementDbContext _context;

        public EmployeeService(ITSupportManagementDbContext context)
        {
            _context = context;
        }

        public async Task AssignEquipmentAsync(Equipment equipment)
        {
            _context.Equipment.Add(equipment);

            await _context.SaveChangesAsync();
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _context.Employee.Add(employee);

            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<List<Equipment>> GetEquipmentsAsync()
        {
            return await _context.Equipment.ToListAsync();
        }
    }
}
