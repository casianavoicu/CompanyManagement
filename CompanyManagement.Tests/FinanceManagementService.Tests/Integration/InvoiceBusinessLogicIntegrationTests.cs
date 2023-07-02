using AutoFixture;
using CompanyManagement.Common.Dto;
using CompanyManagement.MessageIntegration.Constants;
using FinanceManagementWebApi.BusinessLogic;
using FinanceManagementWebApi.Database;
using FinanceManagementWebApi.Database.Entities;
using FinanceManagementWebApi.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System.Reflection;
using System.Text.Json;

namespace CompanyManagement.Tests.FinanceManagementService.Tests.Integration
{
    public class InvoiceBusinessLogicIntegrationTests
    {
        private readonly Fixture _fixture;
        private readonly ServiceProvider _serviceProvider;

        public InvoiceBusinessLogicIntegrationTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<FinanceManagementDbContext>(options =>
                 options.UseInMemoryDatabase(databaseName: "FinanceManagementDbContext"), ServiceLifetime.Transient,
           ServiceLifetime.Transient);

            services.AddTransient<IInvoiceBusinessLogic, InvoiceBusinessLogic>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            _serviceProvider = services.BuildServiceProvider();
            _fixture = new Fixture();
            Setup();
        }

        private void Setup()
        {
            var _dbContext = _serviceProvider
             .GetRequiredService<FinanceManagementDbContext>();

            var result = SeedData();
            _dbContext.Invoice.RemoveRange(_dbContext.Invoice);
            _dbContext.Invoice.AddRange(result);
            _dbContext.SaveChanges();
        }

        [Theory]
        [InlineData(ConstantHelper.HRSupportSender, 1)]
        [InlineData(ConstantHelper.ITSupportSender, 2)]
        public async Task CreateAsync_Should_StoreInDatabase(string sender, int id)
        {
            var invoiceDto = MapData().FirstOrDefault(e => e.Sender == sender);
            var expectedInvoice = SeedData().FirstOrDefault(e => e.Id == id);
            var _dbContext = _serviceProvider
               .GetRequiredService<FinanceManagementDbContext>();

            var invoiceBusinessLogic = _serviceProvider.GetRequiredService<IInvoiceBusinessLogic>();
            var invoiceService = _serviceProvider.GetRequiredService<IInvoiceService>();

            await invoiceBusinessLogic.CreateAsync(invoiceDto!);

            var createdInvoice = await _dbContext.Invoice.FindAsync(id);
            createdInvoice.Should().BeEquivalentTo(expectedInvoice);
            createdInvoice!.InvoiceBody.Should().NotBeNull();
        }

        [Theory]
        [InlineData(ConstantHelper.HRSupportSender)]
        [InlineData(ConstantHelper.ITSupportSender)]
        public async Task CreateAsync_When_Product_Is_Null_Should_Throw_ArgumentNullException(string sender)
        {
            var invoiceDto = MapData().FirstOrDefault(e => e.Sender == sender);
            invoiceDto!.Product = null;
            var _dbContext = _serviceProvider
               .GetRequiredService<FinanceManagementDbContext>();

            var invoiceBusinessLogic = _serviceProvider.GetRequiredService<IInvoiceBusinessLogic>();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await invoiceBusinessLogic.CreateAsync(invoiceDto));
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_ExpectedResult()
        {
            var invoiceBusinessLogic = _serviceProvider.GetRequiredService<IInvoiceBusinessLogic>();

            var result = await invoiceBusinessLogic.GetAllAsync();
            
            result.Value.Should().BeEquivalentTo(SeedData());
            Assert.IsType<ActionResult<List<Invoice>>>(result);
        }

        private List<Invoice> SeedData()
        {
            var firstInvoice = new Invoice
            {
                Id = 1,
                InvoiceBody = JsonSerializer.Serialize(MapData().FirstOrDefault())
            };

            var secondInvoice = new Invoice
            {
                Id = 2,
                InvoiceBody = JsonSerializer.Serialize(MapData().LastOrDefault())
            };

            return new List<Invoice> { firstInvoice, secondInvoice };
        }

        private List<InvoiceDto> MapData()
        {
            var firstInvoice = new InvoiceDto
            {
                Price = 2,
                Product = EquipmentEnum.Laptop.ToString(),
                Sender = ConstantHelper.HRSupportSender
            };
            var secondInvoice = new InvoiceDto
            {
                Price = 2,
                Product = EquipmentEnum.PC.ToString(),
                Sender = ConstantHelper.ITSupportSender
            };

            return new List<InvoiceDto> { firstInvoice, secondInvoice };
        }
    }
}
