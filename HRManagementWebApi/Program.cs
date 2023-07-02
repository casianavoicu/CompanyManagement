using Microsoft.EntityFrameworkCore;
using HRManagementWebApi.Automapper;
using HRManagementWebApi.BusinessLogic;
using HRManagementWebApi.Database;
using HRManagementWebApi.Service;
using CompanyManagement.MessageIntegration;
using System.Diagnostics.CodeAnalysis;
using RabbitMQ.Client;

internal class Program
{
    [ExcludeFromCodeCoverage]

    private static void Main(string[] args)
    {
        Console.WriteLine("Hello from HRManagement");

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<HRManagementDbContext>
                    (options => options.UseInMemoryDatabase("HRManagementDbContext"));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory { HostName = "localhost" });
        builder.Services.AddTransient<IEventingBasicConsumerWrapper, EventingBasicConsumerWrapper>();
        builder.Services.AddTransient<IEmployeeBusinessLogic, EmployeeBusinessLogic>();
        builder.Services.AddTransient<IEmployeeService, EmployeeService>();
        builder.Services.AddAutoMapper(typeof(EmployeeMapperProfile));
        builder.Services.AddSingleton<IPublisherHandler, PublisherHandler>();

        var app = builder.Build();

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