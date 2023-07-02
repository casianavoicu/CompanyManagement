using AutoFixture;
using FinanceManagementWebApi.BusinessLogic;
using FinanceManagementWebApi.Controllers;
using FinanceManagementWebApi.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CompanyManagement.Tests.FinanceManagementService.Tests.Unit
{
    public class InvoiceControllerTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Fixture _fixture;

        public InvoiceControllerTests()
        {
            var services = new ServiceCollection();
            var mockedEmployeeBusinessLogic = Substitute.For<IInvoiceBusinessLogic>();
            services.AddSingleton(mockedEmployeeBusinessLogic);
            _serviceProvider = services.BuildServiceProvider();

            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_ExpectedResult()
        {
            var mockedinvoiceBusinessLogic = _serviceProvider.GetRequiredService<IInvoiceBusinessLogic>();
            var invoiceDtoList = _fixture.Create<List<Invoice>>();
            var invoiceController = new InvoiceController(mockedinvoiceBusinessLogic);
            mockedinvoiceBusinessLogic.GetAllAsync().Returns(invoiceDtoList);

            var result = await invoiceController.GetAllAsync();

            Assert.IsType<ActionResult<List<Invoice>>>(result);
        }
    }
}