using CompanyManagement.Common.Dto;
using CompanyManagement.MessageIntegration;
using System.Text.Json;

namespace ITSuportManagementApi.BusinessLogic
{
    sealed public class UserMessageHandler : IUserMessageHandler
    {
        private readonly IEmployeeBusinessLogic _employeeBusinessLogic;
        private readonly IPublisherHandler _publisherHandler;
        public UserMessageHandler(IEmployeeBusinessLogic employeeBusinessLogic,
            IPublisherHandler publisherHandler)
        {
            _employeeBusinessLogic = employeeBusinessLogic;
            _publisherHandler = publisherHandler;
        }

        public void Process(string message)
        {
            var userDto = JsonSerializer.Deserialize<EmployeeDto>(message);
            _employeeBusinessLogic.HandleRegistrationAsync(userDto!).GetAwaiter().GetResult();

        }
    }

}
