using PeopleHub.Employees.Application.DTOs;

namespace PeopleHub.Employees.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto> GetByIdAsync(Guid id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
    Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto dto);
    Task DeleteAsync(Guid id);
}
