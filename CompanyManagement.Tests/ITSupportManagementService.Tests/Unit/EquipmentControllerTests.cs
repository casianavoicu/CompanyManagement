using AutoFixture;
using CompanyManagement.Common.Dto;
using ITSuportManagementApi.BusinessLogic;
using ITSuportManagementApi.Controllers;
using ITSuportManagementApi.Database.Entities;
using ITSuportManagementApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CompanyManagement.Tests.ITSupportManagementService.Tests.Unit
{
    public class EquipmentControllerTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Fixture _fixture;

        public EquipmentControllerTests()
        {
            var services = new ServiceCollection();
            var mockedEmployeeBusinessLogic = Substitute.For<IEmployeeBusinessLogic>();
            services.AddSingleton(mockedEmployeeBusinessLogic);
            _serviceProvider = services.BuildServiceProvider();

            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_ExpectedResult()
        {
            var mockedEquipmentBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();
            var equipmentList = _fixture.Create<List<Equipment>>();
            var equipmentController = new EquipmentController(mockedEquipmentBusinessLogic);
            mockedEquipmentBusinessLogic.GetEquipmentsAsync().Returns(equipmentList);

            var result = await equipmentController.GetEquipmentsAsync();

            Assert.IsType<ActionResult<List<Equipment>>>(result);
        }

        [Fact]
        public async Task AssignEquipmentAsync_Should_Return_ExpectedResult()
        {
            var mockedEquipmentBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();
            var equipmenteDto = _fixture.Create<InvoiceDto>();
            var equipmentController = new EquipmentController(mockedEquipmentBusinessLogic);
            mockedEquipmentBusinessLogic.HandleEquipmentAsync(Arg.Any<DepartamentEnum>(), Arg.Any<int>()).Returns(equipmenteDto);

            var result = await equipmentController.AssignEquipmentAsync(Arg.Any<DepartamentEnum>(), Arg.Any<int>());

            Assert.IsType<ActionResult<EquipmentDto>>(result);
        }
    }
}
