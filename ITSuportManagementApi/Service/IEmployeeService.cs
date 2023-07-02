using ITSuportManagementApi.Database.Entities;

namespace ITSuportManagementApi.Service
{
    public interface IEmployeeService
    {
        Task<Employee> CreateAsync(Employee employee);

        Task AssignEquipmentAsync(Equipment equipment);

        Task<List<Equipment>> GetEquipmentsAsync();
    }
}