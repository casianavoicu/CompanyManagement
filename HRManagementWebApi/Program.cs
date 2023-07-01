using Microsoft.EntityFrameworkCore;
using HRManagementWebApi.Automapper;
using HRManagementWebApi.BusinessLogic;
using HRManagementWebApi.Database;
using HRManagementWebApi.Service;
using CompanyManagement.MessageIntegration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HRManagementDbContext>
            (options => options.UseInMemoryDatabase("HRManagementDbContext"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
