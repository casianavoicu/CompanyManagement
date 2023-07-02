using CompanyManagement.MessageIntegration;
using CompanyManagement.MessageIntegration.Constants;
using ITSuportManagementApi.BusinessLogic;
using ITSuportManagementApi.Database;
using ITSuportManagementApi.Service;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;

internal class Program
{
    [ExcludeFromCodeCoverage]
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello from ITSupportManagement");

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContext<ITSupportManagementDbContext>
                    (options => options.UseInMemoryDatabase("ITSupportManagementDbContext"), ServiceLifetime.Singleton);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory { HostName = "localhost" });
        builder.Services.AddSingleton<IUserMessageHandler, UserMessageHandler>();
        builder.Services.AddTransient<IQueueHandler, QueueHandler>();
        builder.Services.AddTransient<IEventingBasicConsumerWrapper, EventingBasicConsumerWrapper>();
        builder.Services.AddTransient<IEmployeeBusinessLogic, EmployeeBusinessLogic>();
        builder.Services.AddTransient<IEmployeeService, EmployeeService>();
        builder.Services.AddSingleton<IPublisherHandler, PublisherHandler>();
        var app = builder.Build();


        var userMessageHandler = app.Services.GetRequiredService<IUserMessageHandler>();
        var userQueueHandler = app.Services.GetRequiredService<IQueueHandler>();
        userQueueHandler.Register(ConstantHelper.UserExchange, userMessageHandler);

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