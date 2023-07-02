using AutoFixture;
using CompanyManagement.Common.Dto;
using HRManagementWebApi.BusinessLogic;
using HRManagementWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CompanyManagement.Tests.HRManagement.Tests.Unit
{
    public class EmployeeControllerTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Fixture _fixture;
        
        public EmployeeControllerTests()
        {
            var services = new ServiceCollection();
            var mockedEmployeeBusinessLogic = Substitute.For<IEmployeeBusinessLogic>();
            services.AddSingleton(mockedEmployeeBusinessLogic);
            _serviceProvider = services.BuildServiceProvider();

            _fixture = new Fixture();
        }

        [Fact]
        public async Task RegisterUserAsync_Should_Return_NoContentResult()
        {
            var mockedEmployeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();
            var employee = _fixture.Create<EmployeeLightDto>();
            var employeeController = new EmployeeController(mockedEmployeeBusinessLogic);
            mockedEmployeeBusinessLogic.HandleUserRegistration(employee).Returns(new NoContentResult());

            var result = await employeeController.RegisterUserAsync(employee);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_ExpectedResult()
        {
            var mockedEmployeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();
            var employeeId = _fixture.Create<int>();
            var employeeDto = _fixture.Create<EmployeeDto>();
            var employeeController = new EmployeeController(mockedEmployeeBusinessLogic);
            mockedEmployeeBusinessLogic.GetByIdAsync(employeeId).Returns(employeeDto);

            var result = await employeeController.GetByIdAsync(employeeId);

            Assert.IsType<ActionResult<EmployeeDto>>(result);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_ExpectedResult()
        {
            var mockedEmployeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();
            var employeeDtoList = _fixture.Create<List<EmployeeDto>>();
            var employeeController = new EmployeeController(mockedEmployeeBusinessLogic);
            mockedEmployeeBusinessLogic.GetAllAsync().Returns(employeeDtoList);

            var result = await employeeController.GetAllAsync();

            Assert.IsType<ActionResult<List<EmployeeDto>>>(result);
        }

    }
}
