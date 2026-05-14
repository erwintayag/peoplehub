# PeopleHub — Milestone Summaries

Append-only learning record. One entry per completed milestone.
Format: what was built, concept demonstrated, tradeoffs observed.

---

## M1 — EmployeeService + Docker (completed 2026-05-14)

### What was built
- `Employee.cs` domain entity — private setters, factory `Create()`, `Update()`, `Deactivate()` methods
- `IEmployeeRepository.cs` — interface in Core (zero NuGet deps)
- `NotFoundException.cs` — domain exception in Core
- `EmployeeDto.cs`, `CreateEmployeeDto.cs`, `UpdateEmployeeDto.cs` — C# records
- `IEmployeeService.cs`, `EmployeeService.cs` — Application layer with Mapster mapping
- `MappingConfig.cs` — Mapster `IRegister` implementation
- `AppDbContext.cs` — EF Core DbContext, SQLite, explicit column config
- `EmployeeRepository.cs` — IEmployeeRepository implementation using EF Core
- `ServiceCollectionExtensions.cs` — DI wiring extension method
- Initial EF Core migration (`InitialCreate`) — creates `Employees` table
- `EmployeeEndpoints.cs` — Minimal API CRUD + `/health` via `MapGroup`
- `Program.cs` — clean host setup, DI wiring, Mapster scan
- `appsettings.json` — SQLite connection string
- `Dockerfile` — multi-stage build (sdk:10.0 → aspnet:10.0)
- 7 unit tests with Moq — `EmployeeServiceTests.cs`

### Concept demonstrated
**Containerization** — a .NET service packaged as a self-contained Docker image. The multi-stage build separates the SDK (compile step) from the runtime image (run step), resulting in a smaller production image that does not contain build tools.

**Clean Architecture layers** — `Core → Application → Infrastructure → API`. Each layer depends only on inner layers. `Core` has zero NuGet references — all framework dependencies live in `Infrastructure` and `API`.

**Dependency Inversion** — `EmployeeService` depends on `IEmployeeRepository` (interface in Core), not `EmployeeRepository` (class in Infrastructure). DI wiring happens in `ServiceCollectionExtensions` and `Program.cs`, keeping business logic testable without a real database.

### Tradeoffs observed
- **Namespace collision**: The entity name `Employee` collided with the namespace segment `PeopleHub.Employee.*`. C# namespace resolution wins over `using` aliases when the name matches a namespace segment in the current hierarchy. Fix: rename the root namespace from `PeopleHub.Employee.*` to `PeopleHub.Employees.*` (plural). This also follows idiomatic .NET convention (namespace = plural noun, class = singular noun).
- **EF migration namespace**: `dotnet ef migrations add` auto-generates the migration namespace from the `.csproj` filename, not the `namespace` declarations in source files. After renaming code namespaces, migration files must be manually updated, AND `<RootNamespace>` must be added to the `.csproj` to prevent future migrations from reverting.
- **SQLite + private setters**: EF Core requires a parameterless private constructor on entities with private setters so it can materialize rows from the DB. This is why `Employee` has `private Employee() { }` alongside the public `static Create()` factory.

---
