namespace PeopleHub.Employees.Core.Exceptions;

public class NotFoundException(string entityName, Guid id) : Exception($"{entityName} '{id}' not found.")
{
}
