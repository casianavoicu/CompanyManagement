using AutoFixture;
using AutoMapper;
using CompanyManagement.Common.Dto;
using HRManagementWebApi.BusinessLogic;
using HRManagementWebApi.Database.Entities;
using HRManagementWebApi.Service;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CompanyManagement.Tests.FinanceManagementService.Tests
{
    public class InvoiceBusinessLogicTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Fixture _fixture;

        public InvoiceBusinessLogicTests()
        {
            //var services = new ServiceCollection();
            //var mockedInvoiceService = Substitute.For<IInvoiceService>();
            //services.AddSingleton(mockedInvoiceService);
            //_serviceProvider = services.BuildServiceProvider();

            //_fixture = new Fixture();
        }

        [Fact]
        public async Task GetAll_Should_Return_EmplyeeList_When_EmployeesExist()
        {
         
        }
    }
}
