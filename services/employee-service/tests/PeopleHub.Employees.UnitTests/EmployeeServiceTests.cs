using Mapster;
using Moq;
using PeopleHub.Employees.Application.DTOs;
using PeopleHub.Employees.Application.Mapping;
using PeopleHub.Employees.Application.Services;
using PeopleHub.Employees.Core.Entities;
using PeopleHub.Employees.Core.Exceptions;
using PeopleHub.Employees.Core.Interfaces;

namespace PeopleHub.Employees.UnitTests;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _repoMock;
    private readonly EmployeeService _sut;

    public EmployeeServiceTests()
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(MappingConfig).Assembly);
        _repoMock = new Mock<IEmployeeRepository>();
        _sut = new EmployeeService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEmployees()
    {
        var employees = new List<Employee>
        {
            Employee.Create("Alice", "Smith", "alice@test.com", "Engineering", "Developer"),
            Employee.Create("Bob", "Jones", "bob@test.com", "HR", "Manager")
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);

        var result = await _sut.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFoundException_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Employee?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetByIdAsync(id));
    }

    [Fact]
    public async Task CreateAsync_CreatesAndReturnsDto()
    {
        var dto = new CreateEmployeeDto("Jane", "Doe", "jane@test.com", "Engineering", "Lead");
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);

        var result = await _sut.CreateAsync(dto);

        Assert.Equal("Jane", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.True(result.IsActive);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsNotFoundException_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Employee?)null);
        var dto = new UpdateEmployeeDto("X", "Y", "x@y.com", "IT", "Dev");

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateAsync(id, dto));
    }

    [Fact]
    public async Task DeleteAsync_ThrowsNotFoundException_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Employee?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteAsync(id));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesAndReturnsDto()
    {
        var employee = Employee.Create("Old", "Name", "old@test.com", "Dept", "Role");
        _repoMock.Setup(r => r.GetByIdAsync(employee.Id)).ReturnsAsync(employee);
        _repoMock.Setup(r => r.UpdateAsync(employee)).Returns(Task.CompletedTask);

        var dto = new UpdateEmployeeDto("New", "Name", "new@test.com", "Dept", "Senior Role");
        var result = await _sut.UpdateAsync(employee.Id, dto);

        Assert.Equal("New", result.FirstName);
        Assert.Equal("Senior Role", result.Position);
    }

    [Fact]
    public async Task DeleteAsync_CallsRepository_WhenFound()
    {
        var employee = Employee.Create("Jane", "Doe", "jane@test.com", "HR", "Lead");
        _repoMock.Setup(r => r.GetByIdAsync(employee.Id)).ReturnsAsync(employee);
        _repoMock.Setup(r => r.DeleteAsync(employee.Id)).Returns(Task.CompletedTask);

        await _sut.DeleteAsync(employee.Id);

        _repoMock.Verify(r => r.DeleteAsync(employee.Id), Times.Once);
    }
}
