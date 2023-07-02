using AutoFixture;
using CompanyManagement.Common.Dto;
using CompanyManagement.MessageIntegration;
using CompanyManagement.MessageIntegration.Constants;
using FluentAssertions;
using FluentAssertions.Common;
using HRManagementWebApi.Automapper;
using HRManagementWebApi.Database;
using ITSuportManagementApi.BusinessLogic;
using ITSuportManagementApi.Database;
using ITSuportManagementApi.Database.Entities;
using ITSuportManagementApi.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CompanyManagement.Tests.ITSupportManagementService.Tests.Integration
{
    public class EmployeeBusinessLogicIntegrationTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Fixture _fixture;
        public EmployeeBusinessLogicIntegrationTests()
        {
            var services = new ServiceCollection();
            var mockedPublisherHandler = Substitute.For<IPublisherHandler>();

            services.AddDbContext<ITSupportManagementDbContext>(options =>
              options.UseInMemoryDatabase(databaseName: "ITSupportManagementDbContext"), ServiceLifetime.Transient,
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
        public async Task GetAll_Should_Return_EquipmentsList()
        {
            var expectedResult = SeedEquipmentData();
            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();

            var getAllResult = await employeeBusinessLogic.GetEquipmentsAsync();
            getAllResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData(DepartamentEnum.IT, 400, EquipmentEnum.Laptop)]
        [InlineData(DepartamentEnum.Marketing, 200, EquipmentEnum.PC)]
        public async Task HandleRegistrationAsync_Should_Return_NewInvoice(DepartamentEnum departamentEnum, decimal price, EquipmentEnum equipment)
        {
            var employeeDto = MapEmployeeData().FirstOrDefault(e => e.Departament == departamentEnum);
            
            var expectedDtoResult = new InvoiceDto
            {
                Price = price,
                Product = equipment.ToString(),
                Sender = ConstantHelper.ITSupportSender
            };

            var _employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();

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


        private void Setup()
        {
            var _dbContext = _serviceProvider
                  .GetRequiredService<ITSupportManagementDbContext>();

            var employeeResult = SeedEmployeeData();
            _dbContext.Employee.RemoveRange(_dbContext.Employee);
            _dbContext.Employee.AddRange(employeeResult);
            _dbContext.SaveChanges();

            var equipmentResult = SeedEquipmentData();
            _dbContext.Equipment.RemoveRange(_dbContext.Equipment);
            _dbContext.Equipment.AddRange(equipmentResult);
            _dbContext.SaveChanges();
        }
        private List<Employee> SeedEmployeeData()
        {
            var firstEmployee = new Employee
            {
                Email = "user@gmail.com",
                FirstName = "Test",
                Id = 1,
                LastName = "Test",
            };
            var secondEmployee = new Employee
            {
                Email = "user2@gmail.com",
                FirstName = "Test",
                Id = 2,
                LastName = "Test",
            };

            return new List<Employee> { firstEmployee, secondEmployee };
        }

        private List<EmployeeDto> MapEmployeeData()
        {
            var firstEmployee = new EmployeeDto
            {
                Email = "user@gmail.com",
                FirstName = "Test",
                LastName = "Test",
                Salary = 20,
                EmployeeId = 1,
                Departament = DepartamentEnum.Marketing,
            };
            var secondEmployee = new EmployeeDto
            {
                Email = "user2@gmail.com",
                FirstName = "Test",
                LastName = "Test",
                Salary = 20,
                EmployeeId = 2,
                Departament = DepartamentEnum.IT
            };

            return new List<EmployeeDto> { firstEmployee, secondEmployee };
        }

        private List<Equipment> SeedEquipmentData()
        {
            var firstEmployee = new Equipment
            {
                EmployeeId = SeedEmployeeData().First().Id,
                Id = 1,
                EquipmentPrice = 200,
                EquipmentType = EquipmentEnum.PC

            };
            var secondEmployee = new Equipment
            {
                EmployeeId = SeedEmployeeData().Last().Id,
                Id = 2,
                EquipmentPrice = 400,
                EquipmentType = EquipmentEnum.Laptop
            };

            return new List<Equipment> { firstEmployee, secondEmployee };
        }
       
    }
}
