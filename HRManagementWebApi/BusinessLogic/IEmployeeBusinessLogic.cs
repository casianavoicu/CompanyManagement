using CompanyManagement.Common.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementWebApi.BusinessLogic
{
    public interface IEmployeeBusinessLogic
    {
        Task<ActionResult<List<EmployeeDto>>> GetAllAsync();

        Task<ActionResult<EmployeeDto>> GetByIdAsync(int id);

        Task<ActionResult> HandleUserRegistration(EmployeeLightDto employee);
    }
}