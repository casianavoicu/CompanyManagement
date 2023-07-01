using AutoFixture;
using AutoMapper;
using CompanyManagement.Common.Dto;
using CompanyManagement.MessageIntegration;
using HRManagementWebApi.Automapper;
using HRManagementWebApi.BusinessLogic;
using HRManagementWebApi.Database.Entities;
using HRManagementWebApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace CompanyManagement.Tests.HRManagement.Tests
{
    public class EmployyeBusinessLogicTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Fixture _fixture;

        public EmployyeBusinessLogicTests()
        {
            var services = new ServiceCollection();

            var mockedEmployeeService = Substitute.For<IEmployeeService>();
            var mockedMapper = Substitute.For<IMapper>();

            services.AddSingleton(mockedEmployeeService); 
            services.AddSingleton(mockedMapper);
            services.AddScoped<IEmployeeBusinessLogic, EmployeeBusinessLogic>();

            services.AddAutoMapper(typeof(EmployeeMapperProfile));

            _serviceProvider = services.BuildServiceProvider();

            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAll_Should_Return_EmplyeeList_When_EmployeesExist()
        {
            var expectedDbResult =  _fixture.Create<List<Employee>>();
            var expectedDtoResult = _fixture.Create<List<EmployeeDto>>();

            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();
          
            var mockedEmployeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            mockedEmployeeService.GetAllAsync().Returns(expectedDbResult);

            var mockedMapper = _serviceProvider.GetRequiredService<IMapper>();
            mockedMapper.Map<List<EmployeeDto>>(expectedDbResult).Returns(expectedDtoResult);

            var getAllResult = await employeeBusinessLogic.GetAllAsync();

            Assert.Equal(expectedDtoResult, getAllResult.Value);
        }

        [Fact]
        public async Task GetAll_Should_Return_Null_When_EmployeesDoNotExist()
        {
            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();

            var mockedEmployeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            mockedEmployeeService.GetAllAsync().ReturnsNull();

            var getAllResult = await employeeBusinessLogic.GetAllAsync();

            Assert.IsType<NotFoundResult>(getAllResult.Result);
            Assert.IsType<ActionResult<List<EmployeeDto>>>(getAllResult);
        }

        [Fact]
        public async Task GetById_Should_Return_Employee_When_EmployeeExists()
        {
            var dbResult = _fixture.Create<Employee>();
            var dtoResult = _fixture.Create<EmployeeDto>();

            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();

            var mockedEmployeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            mockedEmployeeService.GetByIdAsync(Arg.Any<int>()).Returns(dbResult);

            var mockedMapper = _serviceProvider.GetRequiredService<IMapper>();
            mockedMapper.Map<EmployeeDto>(dbResult).Returns(dtoResult);

            var getByIdResult = await employeeBusinessLogic.GetByIdAsync(2);

            Assert.Equal(dtoResult, getByIdResult.Value);
            Assert.IsType<ActionResult<EmployeeDto>>(getByIdResult);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_NotFound_WhenUserDoesNotExist()
        {
            var employeeBusinessLogic = _serviceProvider
                .GetRequiredService<IEmployeeBusinessLogic>();

            var mockedEmployeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            mockedEmployeeService.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

            var getByIdResult = await employeeBusinessLogic.GetByIdAsync(2);

            Assert.IsType<NotFoundResult>(getByIdResult.Result);
        }

    }
}
