namespace PeopleHub.Employees.Application.DTOs;

public record EmployeeDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Department,
    string Position,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
