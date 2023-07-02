using AutoFixture;
using CompanyManagement.Common.Dto;
using FinanceManagementWebApi.BusinessLogic;
using FinanceManagementWebApi.Database;
using FinanceManagementWebApi.Database.Entities;
using FinanceManagementWebApi.Service;
using FluentAssertions;
using HRManagementWebApi.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CompanyManagement.Tests.FinanceManagementService.Tests.Integration
{
    public class EmployeeBusinessLogicIntegrationTests
    {
        private readonly Fixture _fixture;
        private readonly ServiceProvider _serviceProvider;

        public EmployeeBusinessLogicIntegrationTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<FinanceManagementDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: "FinanceManagementDbContext"), ServiceLifetime.Transient,
            ServiceLifetime.Transient);

            services.AddTransient<IEmployeeBusinessLogic, EmployeeBusinessLogic>();
            services.AddTransient<IEmployeeService, EmployeeService>();

            _serviceProvider = services.BuildServiceProvider();

            _fixture = new Fixture();
            Setup();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task CreateAsync_Should_Call_ServiceMethod(int id)
        {
            var employeeDto = MapData().FirstOrDefault(e => e.EmployeeId == id);
            var expectedemployee = SeedData().FirstOrDefault(e => e.Id == id);
            var _dbContext = _serviceProvider
               .GetRequiredService<FinanceManagementDbContext>();

            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();
            var employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
          
            await employeeBusinessLogic.CreateAsync(employeeDto!);

            var createdEmployee = await _dbContext.Employee.FindAsync(id);

            createdEmployee.Should().BeEquivalentTo(expectedemployee);
        }

        [Fact]
        public async Task CreateAsync_When_Email_Is_Null_Should_Throw_ArgumentNullException()
        {
            var employeeDto = MapData().FirstOrDefault();
            employeeDto!.Email = null;

            var _dbContext = _serviceProvider
               .GetRequiredService<FinanceManagementDbContext>();

            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await employeeBusinessLogic.CreateAsync(employeeDto));
        }

        private void Setup()
        {
            var _dbContext = _serviceProvider
                  .GetRequiredService<FinanceManagementDbContext>();

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

        private List<EmployeeDto> MapData()
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
