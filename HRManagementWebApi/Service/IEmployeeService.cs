using HRManagementWebApi.Database.Entities;

namespace HRManagementWebApi.Service
{
    public interface IEmployeeService
    {
        Task<Employee> AddAsync(Employee employee);

        Task<Employee> GetByIdAsync(int id);

        Task<List<Employee?>> GetAllAsync();
    }
}