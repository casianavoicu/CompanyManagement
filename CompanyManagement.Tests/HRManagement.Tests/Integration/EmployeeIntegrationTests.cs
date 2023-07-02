using AutoFixture;
using CompanyManagement.Common.Dto;
using CompanyManagement.MessageIntegration;
using FluentAssertions;
using HRManagementWebApi.Automapper;
using HRManagementWebApi.BusinessLogic;
using HRManagementWebApi.Database;
using HRManagementWebApi.Database.Entities;
using HRManagementWebApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CompanyManagement.Tests.HRManagement.Tests.Integration
{
    public class EmployeeIntegrationTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Fixture _fixture;

        public EmployeeIntegrationTests()
        {
            var services = new ServiceCollection();
            var mockedPublisherHandler = Substitute.For<IPublisherHandler>();

            services.AddDbContext<HRManagementDbContext>(options =>
              options.UseInMemoryDatabase(databaseName: "HRManagementDbContext"), ServiceLifetime.Transient,
              ServiceLifetime.Transient);

            services.AddTransient<IEmployeeBusinessLogic, EmployeeBusinessLogic>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddSingleton(mockedPublisherHandler);
            services.AddAutoMapper(typeof(EmployeeMapperProfile));

            _serviceProvider = services.BuildServiceProvider();
            _fixture = new Fixture();
            Setup();
        }

        [Fact]
        public async Task HandleUserRegistration_Should_Return_NoContentResult()
        {
            var employeeBusinessLogic = _serviceProvider
                .GetRequiredService<IEmployeeBusinessLogic>();

            var employeeLightDto = _fixture.Create<EmployeeLightDto>();

            var getByIdResult = await employeeBusinessLogic.HandleUserRegistration(employeeLightDto);

            Assert.IsType<NoContentResult>(getByIdResult);
        }

        [Fact]
        public async Task GetAll_Should_Return_EmployeeList_When_EmployeesExist()
        {
            var expectedResult = MapSeedData();
            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();

            var getAllResult = await employeeBusinessLogic.GetAllAsync();

            expectedResult.Should().BeEquivalentTo(getAllResult.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById_Should_Return_Employee_When_EmployeeExists(int id)
        {
            var expectedResult = MapSeedData().FirstOrDefault(e => e.EmployeeId == id);

            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();

            var getAllResult = await employeeBusinessLogic.GetByIdAsync(id);

            expectedResult.Should().BeEquivalentTo(getAllResult.Value);
        }

        private void Setup()
        {
            var _dbContext = _serviceProvider
                  .GetRequiredService<HRManagementDbContext>();

            var result = SeedData();
            _dbContext.Employee.RemoveRange(_dbContext.Employee);
            _dbContext.Employee.AddRange(result);
            _dbContext.SaveChanges();
        }

        private List<Employee> SeedData()
        {
            var firstEmployee = new Employee
            {
                Email = "user@gmail.com",
                FirstName = "Test",
                Id = 1,
                LastName = "Test",
                Salary = 20,
            };
            var secondEmployee = new Employee
            {
                Email = "user2@gmail.com",
                FirstName = "Test",
                Id = 2,
                LastName = "Test",
                Salary = 20,
            };

            return new List<Employee> { firstEmployee, secondEmployee };
        }

        private List<EmployeeDto> MapSeedData()
        {
            var firstEmployee = new EmployeeDto
            {
                Email = "user@gmail.com",
                FirstName = "Test",
                LastName = "Test",
                Salary = 20,
                EmployeeId = 1
            };
            var secondEmployee = new EmployeeDto
            {
                Email = "user2@gmail.com",
                FirstName = "Test",
                LastName = "Test",
                Salary = 20,
                EmployeeId = 2
            };

            return new List<EmployeeDto> { firstEmployee, secondEmployee };
        }
    }
}