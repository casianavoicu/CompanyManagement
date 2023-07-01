using FinanceManagementWebApi.Database.Entities;

namespace FinanceManagementWebApi.Service
{
    public interface IEmployeeService
    {
        Task CreateAsync(Employee employee);
    }
}
