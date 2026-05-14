using Mapster;
using PeopleHub.Employees.Application.DTOs;
using PeopleHub.Employees.Application.Interfaces;
using PeopleHub.Employees.Core.Entities;
using PeopleHub.Employees.Core.Exceptions;
using PeopleHub.Employees.Core.Interfaces;

namespace PeopleHub.Employees.Application.Services;

public class EmployeeService(IEmployeeRepository repository) : IEmployeeService
{
    private readonly IEmployeeRepository _repository = repository;

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _repository.GetAllAsync();
        return employees.Adapt<IEnumerable<EmployeeDto>>();
    }

    public async Task<EmployeeDto> GetByIdAsync(Guid id)
    {
        var employee = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Employee), id);
        return employee.Adapt<EmployeeDto>();
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        var employee = Employee.Create(
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.Department,
            dto.Position);

        await _repository.AddAsync(employee);
        return employee.Adapt<EmployeeDto>();
    }

    public async Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto dto)
    {
        var employee = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Employee), id);

        employee.Update(dto.FirstName, dto.LastName, dto.Email, dto.Department, dto.Position);
        await _repository.UpdateAsync(employee);
        return employee.Adapt<EmployeeDto>();
    }

    public async Task DeleteAsync(Guid id)
    {
        var employee = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Employee), id);

        await _repository.DeleteAsync(employee.Id);
    }
}
