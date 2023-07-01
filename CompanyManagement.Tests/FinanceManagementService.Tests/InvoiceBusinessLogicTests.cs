using AutoFixture;
using CompanyManagement.Common.Dto;
using FinanceManagementWebApi.BusinessLogic;
using FinanceManagementWebApi.Database;
using FinanceManagementWebApi.Database.Entities;
using FinanceManagementWebApi.Service;
using FluentAssertions;
using ITSuportManagementApi.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CompanyManagement.Tests.FinanceManagementService.Tests
{
    public class InvoiceBusinessLogicTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Fixture _fixture;
        public FinanceManagementDbContext DbContext { get; private set; }

        public InvoiceBusinessLogicTests()
        {
            var services = new ServiceCollection();
            var options = new DbContextOptionsBuilder<FinanceManagementDbContext>()
                      .UseInMemoryDatabase("FinanceManagementDbContext")
                      .Options;
            DbContext = new FinanceManagementDbContext(options);
            var mockeInvoiceService = Substitute.For<IInvoiceService>();

            services.AddScoped<IInvoiceBusinessLogic, InvoiceBusinessLogic>();
            services.AddSingleton(mockeInvoiceService);

            _serviceProvider = services.BuildServiceProvider();

            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateAsync_Should_Call_ServiceMethod()
        {
            var invoice = _fixture.Create<InvoiceDto>();
            var invoiceBusinessLogic = _serviceProvider.GetRequiredService<IInvoiceBusinessLogic>();
            var invoiceService = _serviceProvider.GetRequiredService<IInvoiceService>();
           
            await invoiceBusinessLogic.CreateAsync(invoice);
            await invoiceService.Received(1).CreateAsync(Arg.Any<Invoice>());
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_ArgumentNullException()
        {
            var invoice = _fixture.Create<InvoiceDto>();
            invoice.Product = null;
            var invoiceBusinessLogic = _serviceProvider.GetRequiredService<IInvoiceBusinessLogic>();
            var invoiceService = _serviceProvider.GetRequiredService<IInvoiceService>();

            await invoiceService.Received(0).CreateAsync(Arg.Any<Invoice>());
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await invoiceBusinessLogic.CreateAsync(invoice));
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_InvoiceList()
        {
         
            var invoiceBusinessLogic = _serviceProvider.GetRequiredService<IInvoiceBusinessLogic>();
            var invoiceService = _serviceProvider.GetRequiredService<IInvoiceService>();

            var result =  await invoiceBusinessLogic.GetAllAsync();

            await invoiceService.Received(1).GetAllAsync();
            result.Should().NotBeNull();
        }
    }
}
