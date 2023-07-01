﻿using CompanyManagement.Common.Dto;
using FinanceManagementWebApi.Database.Entities;
using FinanceManagementWebApi.Service;

namespace FinanceManagementWebApi.BusinessLogic
{
    public class EmployeeBusinessLogic : IEmployeeBusinessLogic
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeBusinessLogic(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task CreateAsync(EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                Email= employeeDto.Email,
                FirstName= employeeDto.FirstName,  
                LastName= employeeDto.LastName,
                PaymentDate = DateTime.Now,
                Salary= employeeDto.Salary,
            };
           await _employeeService.CreateAsync(employee);
        }
    }
}
