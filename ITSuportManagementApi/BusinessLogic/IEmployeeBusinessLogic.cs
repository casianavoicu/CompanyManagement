using CompanyManagement.Common.Dto;
using ITSuportManagementApi.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ITSuportManagementApi.BusinessLogic
{
    public interface IEmployeeBusinessLogic
    {
        Task<InvoiceDto> HandleEquipmentAsync(DepartamentEnum departament, int employeeId);

        Task<InvoiceDto> HandleRegistrationAsync(EmployeeDto employeeDto);

        Task<ActionResult<List<Equipment>>>GetEquipmentsAsync();
    }
}
