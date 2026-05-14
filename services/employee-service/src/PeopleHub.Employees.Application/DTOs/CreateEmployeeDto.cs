namespace PeopleHub.Employees.Application.DTOs;

public record CreateEmployeeDto(
    string FirstName,
    string LastName,
    string Email,
    string Department,
    string Position);
