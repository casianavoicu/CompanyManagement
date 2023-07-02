using CompanyManagement.Common.Dto;
using System.Text.Json;

namespace ITSuportManagementApi.BusinessLogic
{
    sealed public class UserMessageHandler : IUserMessageHandler
    {
        private readonly IEmployeeBusinessLogic _employeeBusinessLogic;
        public UserMessageHandler(IEmployeeBusinessLogic employeeBusinessLogic)
        {
            _employeeBusinessLogic = employeeBusinessLogic;
        }

        public void Process(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException("Message is null");

            var userDto = JsonSerializer.Deserialize<EmployeeDto>(message);
            _employeeBusinessLogic.HandleRegistrationAsync(userDto!).GetAwaiter().GetResult();
        }
    }

}
