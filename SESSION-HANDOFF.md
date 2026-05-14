# SESSION-HANDOFF — PeopleHub

## Session Date
2026-05-14 (session 1)

## What Was Done
- Project planned and designed (grill-me session in projects/)
- All Claude meta-files created: CLAUDE.md, CONTEXT.md, SESSION-HANDOFF.md, README.md
- Sprints/SPRINT1.md, Sprints/MILESTONE-SUMMARIES.md created
- .claude/settings.json (stop hook) created
- docker-compose.yml stub created
- .gitignore created
- EmployeeService solution scaffolded: 4 CA layers + 2 test projects + NuGet packages
- Project references wired (Core ← Application ← Infrastructure ← API)
- Boilerplate Class1.cs files removed
- **No application code written yet** — M1 implementation starts next session

## Current State

**Working:**
- Solution builds: `dotnet build` passes (no application code yet — just empty projects)
- All project references correct

**Not started:**
- Employee domain entity, interfaces, repository
- EF Core DbContext + migration
- Application service + DTOs + Mapster mapping
- Minimal API endpoints
- Dockerfile
- `dotnet run` / `docker build` not yet verified

## Next Steps (priority order)
1. **Complete M1 implementation** (in peoplehub/ session):
   - Write `Employee.cs` domain entity in Core (private setters, factory method)
   - Write `IEmployeeRepository.cs` in Core
   - Write `EmployeeDto.cs` + `IEmployeeService.cs` in Application
   - Write `EmployeeService.cs` in Application (Mapster mapping)
   - Write `AppDbContext.cs` + `EmployeeRepository.cs` in Infrastructure
   - Write `ServiceCollectionExtensions.cs` in Infrastructure (DI wiring)
   - Write `EmployeeEndpoints.cs` in API (Minimal API, CRUD + health check)
   - Write `Program.cs` in API
   - Add EF Core migration
   - Write `Dockerfile` (multi-stage build)
   - Verify: `docker build` succeeds + `GET /health` returns 200
2. **Write M1 milestone summary** in `Sprints/MILESTONE-SUMMARIES.md`
3. **Begin M2** (HolidayService + Docker Compose)

## Key Decisions Made
- Stack: .NET 10, SQLite (dev), Docker, YARP gateway, RabbitMQ (M6+)
- 4 initial services: Employee (:5002), Holiday (:5003), Leave (:5004), Auth (:5001)
- Gateway on :5000
- No frontend until M10
- Each service = independent .sln (never mixed)
- No shared DB — ever
- Communication: sync HTTP M1–M5, async events M6+
- Milestone summaries in Sprints/MILESTONE-SUMMARIES.md (append-only)

## Watch Out For
- `Core` project must have ZERO NuGet dependencies — verify before any commit
- `dotnet run` for EmployeeService runs from `src/PeopleHub.Employee.API/`
- Docker build context must be set to `services/employee-service/` (where the .sln is)
- Services reference each other by container name inside Docker, never `localhost`

## Dev State
- Branch: main (peoplehub — no commits yet)
- Tests: no tests written yet
- Build: solution scaffolded, compiles (empty projects)
- Last verified: 2026-05-14 (session 1)
- Active milestone: M1 — EmployeeService + Dockerfile
