using CompanyManagement.Common.Dto;

namespace FinanceManagementWebApi.BusinessLogic
{
    public interface IEmployeeBusinessLogic
    {
        Task CreateAsync(EmployeeDto employeeDto);
    }
}
