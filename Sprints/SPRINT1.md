# Sprint 1 — M1: EmployeeService + M2: HolidayService

## M1 — EmployeeService + Docker
**Concept:** Containerization — packaging a .NET service as a self-contained Docker image.

### Tasks
- [x] Core layer
  - [x] `Employee.cs` — domain entity (private setters, `Create()` factory, `Update()`, `Deactivate()`)
  - [x] `IEmployeeRepository.cs` — interface (GetAll, GetById, Add, Update, Delete)
  - [x] `NotFoundException.cs` — domain exception
- [x] Application layer
  - [x] `EmployeeDto.cs` — response DTO
  - [x] `CreateEmployeeDto.cs`, `UpdateEmployeeDto.cs` — request DTOs
  - [x] `IEmployeeService.cs` — service interface
  - [x] `EmployeeService.cs` — implementation (Mapster mapping, delegates to IEmployeeRepository)
  - [x] `MappingConfig.cs` — Mapster profile (Employee → EmployeeDto)
- [x] Infrastructure layer
  - [x] `AppDbContext.cs` — EF Core DbContext (SQLite)
  - [x] `EmployeeRepository.cs` — IEmployeeRepository implementation
  - [x] `ServiceCollectionExtensions.cs` — DI wiring (AddScoped, AddDbContext)
  - [x] Initial EF migration: `dotnet ef migrations add InitialCreate`
- [x] API layer
  - [x] `Program.cs` — minimal host setup, DI wiring, OpenAPI, middleware
  - [x] `EmployeeEndpoints.cs` — CRUD endpoints + `/health`
  - [x] `appsettings.json` — connection string
- [x] Dockerfile — multi-stage build (sdk → runtime)
- [x] Verify M1 complete:
  - [x] `dotnet build` — no errors
  - [x] `dotnet test` — 7/7 pass (Moq, EmployeeServiceTests)
  - [x] `docker build` — succeeds
  - [x] `docker run` → `GET /health` returns 200
  - [x] `GET /api/v1/employees` returns empty array
  - [x] `POST /api/v1/employees` creates employee, `GET` returns it
- [x] Write M1 summary in `Sprints/MILESTONE-SUMMARIES.md`

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
