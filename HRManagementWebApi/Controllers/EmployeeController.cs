using CompanyManagement.Common.Dto;
using HRManagementWebApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeBusinessLogic _employeeBusinessLogic;

        public EmployeeController(IEmployeeBusinessLogic employeeBusinessLogic)
        {
            _employeeBusinessLogic = employeeBusinessLogic;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterUserAsync(EmployeeLightDto employee)
        {
            return await _employeeBusinessLogic.HandleUserRegistration(employee);
        }

        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<EmployeeDto>> GetByIdAsync(int id)
        {
            return await _employeeBusinessLogic.GetByIdAsync(id);
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<EmployeeDto>>> GetAllAsync()
        {
            return await _employeeBusinessLogic.GetAllAsync();
        }
    }
}
