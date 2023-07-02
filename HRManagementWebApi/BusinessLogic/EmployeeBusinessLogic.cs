using AutoMapper;
using CompanyManagement.Common.Dto;
using CompanyManagement.MessageIntegration;
using CompanyManagement.MessageIntegration.Constants;
using HRManagementWebApi.Database.Entities;
using HRManagementWebApi.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HRManagementWebApi.BusinessLogic
{
    public sealed class EmployeeBusinessLogic : IEmployeeBusinessLogic
    {
        private readonly IEmployeeService _employeeService;
        private IPublisherHandler _publisherHandler;
        private readonly IMapper _mapper;

        public EmployeeBusinessLogic(IEmployeeService employeeService,
            IMapper mapper, IPublisherHandler publisherHandler)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _publisherHandler = publisherHandler;
        }

        public async Task<ActionResult<List<EmployeeDto>>> GetAllAsync()
        {
            var employeeDbResult = await _employeeService.GetAllAsync();

            if (employeeDbResult == null)
            {
                return new NotFoundResult();
            }

            return _mapper.Map<List<EmployeeDto>>(employeeDbResult);
        }

        public async Task<ActionResult<EmployeeDto>> GetByIdAsync(int id)
        {
            var employeeDbResult = await _employeeService.GetByIdAsync(id);

            if (employeeDbResult == null)
            {
                return new NotFoundResult();
            }

            return _mapper.Map<EmployeeDto>(employeeDbResult);
        }

        public async Task<ActionResult> HandleUserRegistration(EmployeeLightDto employee)
        {
            try
            {
                var createdUser = await CreateAsync(employee);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("User Created");
                _publisherHandler.Publish(ConstantHelper.UserExchange, JsonSerializer.Serialize(createdUser.Value));

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new UnprocessableEntityObjectResult(ex.Message);
            }
        }

        private async Task<ActionResult<EmployeeDto>> CreateAsync(EmployeeLightDto employee)
        {
            var mappedUser = _mapper.Map<Employee>(employee);

            var createdUser = await _employeeService.AddAsync(mappedUser);

            if (createdUser.Id == default)
                return new BadRequestResult();

            return _mapper.Map<EmployeeDto>(createdUser);
        }
    }
}