using Mapster;
using PeopleHub.Employees.Application.DTOs;
using PeopleHub.Employees.Core.Entities;

namespace PeopleHub.Employees.Application.Mapping;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Employee, EmployeeDto>();
    }
}
