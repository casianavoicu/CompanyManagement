using CompanyManagement.Common.Dto;
using FinanceManagementWebApi.BusinessLogic;
using FinanceManagementWebApi.Handler;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Text.Json;

namespace CompanyManagement.Tests.FinanceManagementService.Tests.Unit
{
    public class UserMessageHandlerTests
    {
        private readonly ServiceProvider _serviceProvider;

        public UserMessageHandlerTests()
        {
            var services = new ServiceCollection();
            var mockedInvoiceBusinessLogic = Substitute.For<IEmployeeBusinessLogic>();
            services.AddSingleton(mockedInvoiceBusinessLogic);
            services.AddScoped<IUserMessageHandler, UserMessageHandler>();
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void Process_Should_Create_New_User_When_Message_NotIsEmpty()
        {
            var employeeMessage = new EmployeeDto
            {
                Email = "user@gmail.com",
                FirstName = "Test",
                LastName = "Test",
                Salary = 20,
                EmployeeId = 1
            };

            var message = JsonSerializer.Serialize(employeeMessage);

            var userMessageHandler = _serviceProvider.GetRequiredService<IUserMessageHandler>();

            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();

            userMessageHandler.Process(message);

            employeeBusinessLogic.Received(1).CreateAsync(Arg.Any<EmployeeDto>());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Process_Should_Throw_Argument_Null_Exception_When_Message_IsEmpty(string? message)
        {
            var userMessageHandler = _serviceProvider.GetRequiredService<IUserMessageHandler>();

            var employeeBusinessLogic = _serviceProvider.GetRequiredService<IEmployeeBusinessLogic>();

            Assert.Throws<ArgumentNullException>(() => userMessageHandler.Process(message));

            employeeBusinessLogic.Received(0).CreateAsync(Arg.Any<EmployeeDto>());
        }
    }
}