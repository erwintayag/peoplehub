using Microsoft.EntityFrameworkCore;
using PeopleHub.Employees.Core.Entities;
using PeopleHub.Employees.Core.Interfaces;
using PeopleHub.Employees.Infrastructure.Persistence;

namespace PeopleHub.Employees.Infrastructure.Repositories;

public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await context.Employees.ToListAsync();

    public async Task<Employee?> GetByIdAsync(Guid id)
        => await context.Employees.FindAsync(id);

    public async Task AddAsync(Employee employee)
    {
        await context.Employees.AddAsync(employee);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        context.Employees.Update(employee);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee is not null)
        {
            context.Employees.Remove(employee);
            await context.SaveChangesAsync();
        }
    }
}
