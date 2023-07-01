using CompanyManagement.Common.Dto;
using CompanyManagement.MessageIntegration;
using CompanyManagement.MessageIntegration.Constants;
using ITSuportManagementApi.Database.Entities;
using ITSuportManagementApi.Dto;
using ITSuportManagementApi.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ITSuportManagementApi.BusinessLogic
{
    sealed public class EmployeeBusinessLogic : IEmployeeBusinessLogic
    {
        private readonly IEmployeeService _employeeService;
        private readonly Dictionary<int, decimal> _equipmentProducts
            = new Dictionary<int, decimal>() { { (int)EquipmentEnum.PC, 200 }, { (int)EquipmentEnum.Laptop, 400 } };
        private readonly IPublisherHandler _publisherHandler;
        public EmployeeBusinessLogic(IEmployeeService employeeService, IPublisherHandler publisherHandler)
        {
            _employeeService = employeeService;
            _publisherHandler = publisherHandler;
        }

        public async Task<InvoiceDto> HandleRegistrationAsync(EmployeeDto employeeDto)
        {
            var mappedEmployee = new Employee
            {
                Email = employeeDto.Email,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
            };
            await _employeeService.CreateAsync(mappedEmployee);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("User Created");

            return await HandleEquipmentAsync(employeeDto.Departament, mappedEmployee.Id);
        }

        public async Task<InvoiceDto> HandleEquipmentAsync(DepartamentEnum departament, int employeeId)
        {
            var equipment = await AssignEquipment(departament, employeeId);

            var invoice = new InvoiceDto
            {
                CreatedDate = DateTime.Now,
                Price = equipment.EquipmentPrice,
                Product = equipment.EquipmentType.ToString(),
                Sender = ConstantHelper.ITSupportSender,
            };

            _publisherHandler.Publish(ConstantHelper.InvoiceExchange, JsonSerializer.Serialize(invoice));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Invoice sent");
            
            return invoice;
        }

        private async Task<EquipmentDto> AssignEquipment(DepartamentEnum departament, int employeeId)
        {
            var equipmentType = departament == DepartamentEnum.IT ? EquipmentEnum.Laptop : EquipmentEnum.PC;

            var equipment = new Equipment { EmployeeId = employeeId, EquipmentPrice = _equipmentProducts.GetValueOrDefault((int)equipmentType), EquipmentType = equipmentType };
           
            await _employeeService.AssignEquipmentAsync(equipment);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Equipment Assigned");

            return new EquipmentDto { EmployeeId = employeeId, EquipmentPrice = equipment.EquipmentPrice, EquipmentType = equipment.EquipmentType };
        }

        public async Task<ActionResult<List<Equipment>>> GetAllAsync()
        {
            return await _employeeService.GetAllAsync();
        }
    }
}
