using CompanyManagement.Common.Dto;
using ITSuportManagementApi.BusinessLogic;
using ITSuportManagementApi.Database.Entities;
using ITSuportManagementApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ITSuportManagementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly IEmployeeBusinessLogic _employeeBusinessLogic;

        public EquipmentController(IEmployeeBusinessLogic employeeBusinessLogic)
        {
            _employeeBusinessLogic = employeeBusinessLogic;
        }

        [HttpPost]
        [Route("assign")]
        public async Task<ActionResult<EquipmentDto>> AssignEquipmentAsync(DepartamentEnum departament, int employeeId)
        {
            var result = await _employeeBusinessLogic.HandleEquipmentAsync(departament, employeeId);

            return Ok(result);
        }

        [HttpGet]
        [Route("getEquipments")]
        public async Task<ActionResult<List<Equipment>>> GetEquipmentsAsync()
        {
            var result = await _employeeBusinessLogic.GetEquipmentsAsync();

            return Ok(result);
        }
    }
}
