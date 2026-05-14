# Sprint 1 — M1: EmployeeService + M2: HolidayService

## M1 — EmployeeService + Docker
**Concept:** Containerization — packaging a .NET service as a self-contained Docker image.

### Tasks
- [ ] Core layer
  - [ ] `Employee.cs` — domain entity (private setters, `Create()` factory, `Update()`, `Deactivate()`)
  - [ ] `IEmployeeRepository.cs` — interface (GetAll, GetById, Add, Update, Delete)
  - [ ] `NotFoundException.cs` — domain exception
- [ ] Application layer
  - [ ] `EmployeeDto.cs` — response DTO
  - [ ] `CreateEmployeeDto.cs`, `UpdateEmployeeDto.cs` — request DTOs
  - [ ] `IEmployeeService.cs` — service interface
  - [ ] `EmployeeService.cs` — implementation (Mapster mapping, delegates to IEmployeeRepository)
  - [ ] `MappingConfig.cs` — Mapster profile (Employee → EmployeeDto)
- [ ] Infrastructure layer
  - [ ] `AppDbContext.cs` — EF Core DbContext (SQLite)
  - [ ] `EmployeeRepository.cs` — IEmployeeRepository implementation
  - [ ] `ServiceCollectionExtensions.cs` — DI wiring (AddScoped, AddDbContext)
  - [ ] Initial EF migration: `dotnet ef migrations add InitialCreate`
- [ ] API layer
  - [ ] `Program.cs` — minimal host setup, DI wiring, Swagger, middleware
  - [ ] `EmployeeEndpoints.cs` — CRUD endpoints + `/health`
  - [ ] `appsettings.json` — connection string, port config
- [ ] Dockerfile — multi-stage build (sdk → runtime)
- [ ] Verify M1 complete:
  - [ ] `dotnet build` — no errors
  - [ ] `dotnet test` — all pass (at least 1 unit test per service method)
  - [ ] `docker build` — succeeds
  - [ ] `docker run` → `GET /health` returns 200
  - [ ] `GET /api/v1/employees` returns empty array
  - [ ] `POST /api/v1/employees` creates employee, `GET` returns it
- [ ] Write M1 summary in `Sprints/MILESTONE-SUMMARIES.md`

---

## M2 — HolidayService + Docker Compose
**Concept:** Multi-container orchestration — services discovering each other by name.

### Tasks
- [ ] Scaffold HolidayService (same 4-layer structure as EmployeeService)
  - [ ] `Holiday.cs` domain entity
  - [ ] CRUD endpoints + `/health`
  - [ ] `GET /api/v1/holidays/check?startDate=&endDate=` — overlap check endpoint
  - [ ] Dockerfile
- [ ] `docker-compose.yml` — wire employee-service + holiday-service
  - [ ] Named volumes for SQLite persistence
  - [ ] `healthcheck:` per service using `/health`
  - [ ] Internal network: `peoplehub-net`
- [ ] Verify M2 complete:
  - [ ] `docker compose up` starts both services
  - [ ] Both `/health` endpoints return 200
  - [ ] Services reachable through their container names on `peoplehub-net`
  - [ ] `docker compose down && docker compose up` — data persists (volumes)
- [ ] Write M2 summary in `Sprints/MILESTONE-SUMMARIES.md`
