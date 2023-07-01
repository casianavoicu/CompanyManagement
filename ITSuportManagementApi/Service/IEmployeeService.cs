using ITSuportManagementApi.Database.Entities;

namespace ITSuportManagementApi.Service
{
    public interface IEmployeeService
    {
        Task CreateAsync(Employee employee);
        Task AssignEquipmentAsync(Equipment equipment);
        Task<List<Equipment>> GetAllAsync();
    }
}
