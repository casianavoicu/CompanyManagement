using AutoFixture;
using CompanyManagement.Common.Dto;
using FinanceManagementWebApi.BusinessLogic;
using FinanceManagementWebApi.Database;
using FinanceManagementWebApi.Database.Entities;
using FinanceManagementWebApi.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CompanyManagement.Tests.FinanceManagementService.Tests.Unit
{
    public class FinanceEmployeeBusinessLogicTests
    {
        private readonly Fixture _fixture;
        private readonly ServiceProvider _serviceProvider;

        public FinanceManagementDbContext DbContext { get; private set; }

        public FinanceEmployeeBusinessLogicTests()
        {
            var services = new ServiceCollection();
            var options = new DbContextOptionsBuilder<FinanceManagementDbContext>()
                      .UseInMemoryDatabase("FinanceManagementDbContext")
                      .Options;

            DbContext = new FinanceManagementDbContext(options);
            var mockedEmployeeService = Substitute.For<IEmployeeService>();

            services.AddScoped<IEmployeeBusinessLogic, EmployeeBusinessLogic>();
            services.AddSingleton(mockedEmployeeService);

            _serviceProvider = services.BuildServiceProvider();

            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateAsync_Should_Call_ServiceMethod()
        {
            var employee = _fixture.Create<EmployeeDto>();
            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();
            var employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();

            await employeeBusinessLogic.CreateAsync(employee);
            await employeeService.Received(1).CreateAsync(Arg.Any<Employee>());
        }

        [Fact]
        public async Task CreateAsync_When_Email_Is_Null_Should_Throw_ArgumentNullException()
        {
            var employee = _fixture.Create<EmployeeDto>();
            employee.Email = null;

            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();
            var employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();

            await employeeService.Received(0).CreateAsync(Arg.Any<Employee>());
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await employeeBusinessLogic.CreateAsync(employee));
        }
    }
}