using CompanyManagement.Common.Dto;
using FinanceManagementWebApi.BusinessLogic;
using System.Text.Json;

namespace FinanceManagementWebApi.Handler
{
    sealed  public class UserMessageHandler : IUserMessageHandler
    {
        private readonly IEmployeeBusinessLogic _employeeBusinessLogic;

        public UserMessageHandler(IEmployeeBusinessLogic employeeBusinessLogic)
        {
            _employeeBusinessLogic = employeeBusinessLogic;
        }

        public void Process(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Message Received");

            var deserializedEmployee = JsonSerializer.Deserialize<EmployeeDto>(message);
            _employeeBusinessLogic.CreateAsync(deserializedEmployee!).GetAwaiter().GetResult();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("User Created");
        }
    }
}
