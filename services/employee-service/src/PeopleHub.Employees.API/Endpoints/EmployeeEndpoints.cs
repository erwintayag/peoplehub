using PeopleHub.Employees.Application.DTOs;
using PeopleHub.Employees.Application.Interfaces;
using PeopleHub.Employees.Core.Exceptions;

namespace PeopleHub.Employees.API.Endpoints;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        app.MapGet("/health", () => Results.Ok("Healthy"))
            .WithName("Health")
            .WithTags("Health");

        var group = app.MapGroup("/api/v1/employees").WithTags("Employees");

        group.MapGet("/", async (IEmployeeService service) =>
        {
            var employees = await service.GetAllAsync();
            return Results.Ok(employees);
        });

        group.MapGet("/{id:guid}", async (Guid id, IEmployeeService service) =>
        {
            try
            {
                var employee = await service.GetByIdAsync(id);
                return Results.Ok(employee);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(new { ex.Message });
            }
        });

        group.MapPost("/", async (CreateEmployeeDto dto, IEmployeeService service) =>
        {
            var employee = await service.CreateAsync(dto);
            return Results.Created($"/api/v1/employees/{employee.Id}", employee);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateEmployeeDto dto, IEmployeeService service) =>
        {
            try
            {
                var employee = await service.UpdateAsync(id, dto);
                return Results.Ok(employee);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(new { ex.Message });
            }
        });

        group.MapDelete("/{id:guid}", async (Guid id, IEmployeeService service) =>
        {
            try
            {
                await service.DeleteAsync(id);
                return Results.NoContent();
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(new { ex.Message });
            }
        });
    }
}
