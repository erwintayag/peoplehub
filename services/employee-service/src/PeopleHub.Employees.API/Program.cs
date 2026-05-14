using Mapster;
using PeopleHub.Employees.API.Endpoints;
using PeopleHub.Employees.Application.Interfaces;
using PeopleHub.Employees.Application.Mapping;
using PeopleHub.Employees.Application.Services;
using PeopleHub.Employees.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var mapsterConfig = TypeAdapterConfig.GlobalSettings;
mapsterConfig.Scan(typeof(MappingConfig).Assembly);

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapEmployeeEndpoints();

app.Run();
