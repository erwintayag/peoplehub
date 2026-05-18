# SESSION-HANDOFF — PeopleHub

## Session Date
2026-05-18 (session 3)

## What Was Done
- Session 1 (2026-05-14): Project planned, meta-files created, solution scaffolded (empty layers)
- Session 2 (2026-05-14): **M1 complete** ✅
- Session 3 (2026-05-18): M1 knowledge review (grill session). Updated CLAUDE.md + memory with **code-along learning style rule** — user writes code from M2+, Claude guides/reviews only.
  - `git init -b main` in `peoplehub/` — repo now independent (no GitHub remote yet, user must create)
  - Namespace and folder rename: `PeopleHub.Employee.*` → `PeopleHub.Employees.*` to eliminate entity/namespace collision
  - All 4 CA layers implemented: Core → Application → Infrastructure → API
  - EF Core migration `InitialCreate` added and applied (SQLite, employees.db)
  - 7 unit tests with Moq — all pass
  - `dotnet build` ✓ `dotnet test` (7/7) ✓ `docker build` ✓ `/health` ✓ CRUD ✓
  - `<RootNamespace>PeopleHub.Employees.Infrastructure</RootNamespace>` added to Infrastructure.csproj

## Current State

**Working:**
- `peoplehub/` — git initialized on `main`, no remote yet
- EmployeeService — M1 complete, fully functional, containerized
- `dotnet run` → http://localhost:5237
- `docker run -p 5002:8080 peoplehub-employee` → `/health` returns 200

**Uncommitted:**
- Everything in `peoplehub/` (user commits manually — no Claude commits)

**Not done yet:**
- GitHub repo `peoplehub` creation (user must create remotely, then `git remote add origin ...`)
- M2 — HolidayService + Docker Compose
- Integration test suite (placeholder only)

## Next Steps (priority order)
1. **Create GitHub repo** named `peoplehub` (on GitHub), then:
   ```
   cd peoplehub/
   git remote add origin https://github.com/<username>/peoplehub.git
   git push -u origin main
   ```
2. **Begin M2** — HolidayService + Docker Compose:
   - Scaffold HolidayService (same 4-layer structure as EmployeeService)
   - `Holiday.cs` domain entity: `Id`, `Name`, `Date`, `IsRecurring`
   - CRUD endpoints + `/health`
   - `GET /api/v1/holidays/check?startDate=&endDate=` — overlap check endpoint
   - Dockerfile
   - Update `docker-compose.yml` — wire employee-service + holiday-service
   - Named volumes, healthchecks, `peoplehub-net` network

## Key Decisions Made
- **Code-along mode (M2+):** User writes all code. Claude explains what/why, reviews attempts, fills gaps only when user is stuck or explicitly asks. Rule in CLAUDE.md + memory.
- Namespace: `PeopleHub.Employees.*` (plural) — avoids entity/namespace collision
- `<RootNamespace>` must be set in each `.csproj` that uses the `Employees` namespace to prevent EF migrations from reverting it
- git commits are user-managed — Claude does not commit or push
- GitHub remote: not yet configured (user task)
- Migration path: run `dotnet ef database update` from `src/PeopleHub.Employees.API/`
- Port: EmployeeService runs on :5237 (dev), :5002 (Docker)

## Watch Out For
- Every future service must use `PeopleHub.{ServiceName}s.*` namespace (plural) to avoid the same collision
- EF migration namespace will default to the `.csproj` filename — always check generated migration files and set `<RootNamespace>` in the `.csproj` before running `dotnet ef migrations add`
- Integration test project (`PeopleHub.Employees.IntegrationTests`) still has placeholder `UnitTest1.cs` — not yet written

## Dev State
- Branch: main (peoplehub — commits pending, user handles)
- Tests: 7/7 unit tests pass
- Build: clean (`dotnet build` 0 errors, 0 warnings)
- Docker: image `peoplehub-employee` built and verified
- Last verified: 2026-05-18 (session 3)
- Active milestone: M2 — HolidayService + Docker Compose
