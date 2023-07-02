using AutoFixture;
using CompanyManagement.Common.Dto;
using CompanyManagement.MessageIntegration;
using CompanyManagement.MessageIntegration.Constants;
using FluentAssertions;
using ITSuportManagementApi.BusinessLogic;
using ITSuportManagementApi.Database;
using ITSuportManagementApi.Database.Entities;
using ITSuportManagementApi.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CompanyManagement.Tests.ITSupportManagementService.Tests
{
    public class EmployeeBusinessLogicTests
    {
        private readonly Fixture _fixture;
        private readonly ServiceProvider _serviceProvider;
        public ITSupportManagementDbContext DbContext { get; private set; }

        public EmployeeBusinessLogicTests()
        {
            var services = new ServiceCollection();
            var options = new DbContextOptionsBuilder<ITSupportManagementDbContext>()
                      .UseInMemoryDatabase("ITSupportManagementDbContext")
                      .Options;

            DbContext = new ITSupportManagementDbContext(options);

            var mockedEmployeeService = Substitute.For<IEmployeeService>();
            var mockedPublisherHandler = Substitute.For<IPublisherHandler>();

            services.AddScoped<IEmployeeBusinessLogic, EmployeeBusinessLogic>();
            services.AddSingleton(mockedEmployeeService);
            services.AddSingleton(mockedPublisherHandler);

            _serviceProvider = services.BuildServiceProvider();

            _fixture = new Fixture();
        }

        [Theory]
        [InlineData(DepartamentEnum.IT, 400, EquipmentEnum.Laptop)]
        [InlineData(DepartamentEnum.Marketing, 200, EquipmentEnum.PC)]
        public async Task HandleRegistrationAsync_Should_Return_NewInvoice(DepartamentEnum departamentEnum, decimal price, EquipmentEnum equipment)
        {
            var employee = _fixture.Create<Employee>();
            var employeeDto = new EmployeeDto
            {
                Departament = departamentEnum,
                Email = "user@gmail.com",
                FirstName = "John",
                LastName = "Smith",
                Salary = 200,
            };
            var expectedDtoResult = new InvoiceDto
            {
                Price = price,
                Product = equipment.ToString(),
                Sender = ConstantHelper.ITSupportSender
            };

            var _employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();
            var _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();

            _employeeService.CreateAsync(Arg.Any<Employee>()).Returns(employee);
            var result = await _employeeBusinessLogic.HandleRegistrationAsync(employeeDto);

            result.Should().BeEquivalentTo(expectedDtoResult);
        }

        [Theory]
        [InlineData(DepartamentEnum.IT, 400, EquipmentEnum.Laptop)]
        [InlineData(DepartamentEnum.Marketing, 200, EquipmentEnum.PC)]
        public async Task HandleEquipmentAsync_Should_Return_NewInvoice(DepartamentEnum departamentEnum, decimal price, EquipmentEnum equipmentType)
        {

            var expectedDtoResult = new InvoiceDto
            {
                Price = price,
                Product = equipmentType.ToString(),
                Sender = ConstantHelper.ITSupportSender
            };

            var employeeId = _fixture.Create<int>();

            var _employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();

            var result = await _employeeBusinessLogic.HandleEquipmentAsync(departamentEnum, employeeId);

            result.Should().BeEquivalentTo(expectedDtoResult);
        }

        [Theory]
        [InlineData(DepartamentEnum.IT, 400, EquipmentEnum.Laptop)]
        [InlineData(DepartamentEnum.Marketing, 200, EquipmentEnum.PC)]
        public async Task HandleEquipmentAsync_Should_ThrowException_When_EmployeeId_Is_Null(DepartamentEnum departamentEnum, decimal price, EquipmentEnum equipmentType)
        {
            var expectedDtoResult = new InvoiceDto
            {
                Price = price,
                Product = equipmentType.ToString(),
                Sender = ConstantHelper.ITSupportSender
            };

            var employeeId = 0;

            var _employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _employeeBusinessLogic.HandleEquipmentAsync(departamentEnum, employeeId));

        }

    }
}
