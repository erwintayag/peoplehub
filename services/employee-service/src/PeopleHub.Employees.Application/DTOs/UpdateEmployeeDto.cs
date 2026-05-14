namespace PeopleHub.Employees.Application.DTOs;

public record UpdateEmployeeDto(
    string FirstName,
    string LastName,
    string Email,
    string Department,
    string Position);
