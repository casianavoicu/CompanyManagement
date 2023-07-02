using CompanyManagement.Common.Dto;
using CompanyManagement.MessageIntegration.Constants;
using FinanceManagementWebApi.BusinessLogic;
using FinanceManagementWebApi.Handler;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Text.Json;

namespace CompanyManagement.Tests.FinanceManagementService.Tests.Unit
{
    public class InvoiceMessageHandlerTests
    {
        private readonly ServiceProvider _serviceProvider;

        public InvoiceMessageHandlerTests()
        {
            var services = new ServiceCollection();
            var mockedInvoiceBusinessLogic = Substitute.For<IInvoiceBusinessLogic>();
            services.AddSingleton(mockedInvoiceBusinessLogic);
            services.AddScoped<IInvoiceMessageHandler, InvoiceMessageHandler>();
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void Process_Should_Create_New_Invoice_When_Message_NotIsEmpty()
        {
            var invoiceMessage = new InvoiceDto
            {
                Price = 2,
                Product = EquipmentEnum.Laptop.ToString(),
                Sender = ConstantHelper.HRSupportSender
            };
            var message = JsonSerializer.Serialize(invoiceMessage);

            var invoiceMessageHandler = _serviceProvider.GetRequiredService<IInvoiceMessageHandler>();

            var invoiceBusinessLogic = _serviceProvider.GetRequiredService<IInvoiceBusinessLogic>();

            invoiceMessageHandler.Process(message);

            invoiceBusinessLogic.Received(1).CreateAsync(Arg.Any<InvoiceDto>());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Process_Should_Throw_Argument_Null_Exception_When_Message_IsEmpty(string? message)
        {
            var invoiceMessageHandler = _serviceProvider.GetRequiredService<IInvoiceMessageHandler>();

            var invoiceBusinessLogic = _serviceProvider.GetRequiredService<IInvoiceBusinessLogic>();

            Assert.Throws<ArgumentNullException>(() => invoiceMessageHandler.Process(message));

            invoiceBusinessLogic.Received(0).CreateAsync(Arg.Any<InvoiceDto>());
        }
    }
}