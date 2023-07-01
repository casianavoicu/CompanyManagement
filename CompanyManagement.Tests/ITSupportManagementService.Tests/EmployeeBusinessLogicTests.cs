using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CompanyManagement.Tests.ITSupportManagementService.Tests
{
    public class EmployeeBusinessLogicTests
    {
        private readonly Fixture _fixture;
        private readonly ServiceProvider _serviceProvider;
        public EmployeeBusinessLogicTests()
        {
            var services = new ServiceCollection();
            
        }

        [Fact]
        public async Task HandleRegistrationAsync_Should_Return_NewInvoice()
        {
            //Arrange

            //Assert

            //Acti

        }
    }
}
