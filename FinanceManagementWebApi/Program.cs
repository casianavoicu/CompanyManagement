using CompanyManagement.MessageIntegration;
using CompanyManagement.MessageIntegration.Constants;
using FinanceManagementWebApi.BusinessLogic;
using FinanceManagementWebApi.Database;
using FinanceManagementWebApi.Handler;
using FinanceManagementWebApi.Service;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;

internal class Program
{
    [ExcludeFromCodeCoverage]
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello from FinanceManagement");

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContext<FinanceManagementDbContext>
                    (options => options.UseInMemoryDatabase("FinanceManagementDbContext"), ServiceLifetime.Singleton);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" });
        builder.Services.AddTransient<IQueueHandler, QueueHandler>();
        builder.Services.AddSingleton<IInvoiceMessageHandler, InvoiceMessageHandler>();
        builder.Services.AddSingleton<IUserMessageHandler, UserMessageHandler>();
        builder.Services.AddTransient<IEventingBasicConsumerWrapper, EventingBasicConsumerWrapper>();
        builder.Services.AddTransient<IEmployeeBusinessLogic, EmployeeBusinessLogic>();
        builder.Services.AddTransient<IInvoiceBusinessLogic, InvoiceBusinessLogic>();
        builder.Services.AddTransient<IInvoiceService, InvoiceService>();
        builder.Services.AddTransient<IEmployeeService, EmployeeService>();
        var app = builder.Build();

        var userMessageHandler = app.Services.GetRequiredService<IUserMessageHandler>();
        var invoiceMessageHandler = app.Services.GetRequiredService<IInvoiceMessageHandler>();

        var userQueueHandler = app.Services.GetRequiredService<IQueueHandler>();
        var invoiceQueueHandler = app.Services.GetRequiredService<IQueueHandler>();

        userQueueHandler.Register(ConstantHelper.UserExchange, userMessageHandler);
        invoiceQueueHandler.Register(ConstantHelper.InvoiceExchange, invoiceMessageHandler);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.MapControllers();

        app.Run();
    }
}