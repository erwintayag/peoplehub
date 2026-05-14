# PeopleHub — Claude Quick Reference

## What This Project Is

Microservices learning project. HR domain (Employee, Holiday, Leave, Auth services).
Purpose: build confidence working with microservices in greenfield and brownfield contexts.
Learning style: hands-on first, concepts explained as each milestone is built.

**This is NOT a production project.** Architectural decisions are deliberate teaching choices.

---

## How to Start a Session

```
1. Read SESSION-HANDOFF.md — current milestone, what's in progress, what's next
2. Read Sprints/SPRINT1.md (or current sprint) — task list for the active milestone
3. Check: git status — no unexpected changes
4. Run: docker compose up (if M2+) or dotnet run in the active service (M1)
5. Begin work
```

---

## Dev Commands

| Command | What |
|---------|------|
| `cd services/employee-service && dotnet build` | Build EmployeeService |
| `cd services/employee-service && dotnet test` | Run all tests |
| `cd services/employee-service/src/PeopleHub.Employee.API && dotnet run` | Run EmployeeService locally |
| `docker build -t peoplehub-employee ./services/employee-service` | Build Docker image |
| `docker run -p 5002:8080 peoplehub-employee` | Run container (M1) |
| `docker compose up` | Start all services (M2+) |
| `docker compose down` | Stop all services |

---

## Architecture Rules (from projects/PLAYBOOK.md §6)

> Full spec: `../PLAYBOOK.md` Section 6. This section is a distillation — not a replacement.
> When `../PLAYBOOK.md §6` is updated, this section must be synced in the same `projects/` session.

### Clean Architecture — layer structure (non-negotiable)

```
Core → Application → Infrastructure → API
```

| Layer | Owns | Must NOT contain |
|-------|------|-----------------|
| **Core** | Domain entities, interfaces, exceptions | Any NuGet package reference |
| **Application** | Services, DTOs, mapping config | EF Core, HTTP types |
| **Infrastructure** | EF Core DbContext, repositories | Business logic, HTTP controllers |
| **API** | Minimal API endpoints, DI wiring, Program.cs | Direct DbContext access, business logic |

**Hard violations — these fail PR review:**
- `Core` project has any NuGet `<PackageReference>` → fix immediately
- Service class imports `Microsoft.EntityFrameworkCore` → move to Infrastructure
- Controller/endpoint calls DbContext directly → route through service
- Domain entity returned from endpoint response → map to DTO first

### OOP

- Private setters on domain entities — domain methods mutate state, not public setters
- Prefer composition over inheritance — max 2 inheritance levels
- Interfaces for all cross-layer dependencies

### SOLID (enforced at PR review)

| Principle | Rule |
|-----------|------|
| **SRP** | One class, one reason to change. Name contains "And"/"Manager"? Likely a violation. |
| **OCP** | New behavior via new code, not modifying existing classes |
| **LSP** | No `NotImplementedException` in overrides |
| **ISP** | No interface with >7 methods |
| **DIP** | Constructor injection always. No `new ConcreteService()` in Application or Core |

---

## Microservices Rules (peoplehub-specific)

### Service boundaries
- Each service = independent `.sln` — never mix two services in one solution
- Services communicate via **HTTP only** during M1–M5
- Services communicate via **RabbitMQ events** starting M6 — do not introduce async before M6
- **No shared database** — each service owns its own SQLite file; no cross-service table joins, ever
- Services reference each other by **container name** in Docker Compose, never by `localhost`

### Port map (fixed — matches docker-compose.yml)

| Service | Port |
|---------|------|
| Gateway (YARP) | :5000 |
| AuthService | :5001 |
| EmployeeService | :5002 |
| HolidayService | :5003 |
| LeaveService | :5004 |

### Current milestone rule
Only build what the current milestone requires. Do not introduce Docker Compose before M2,
do not introduce async messaging before M6, do not introduce the gateway before M4.
Check `SESSION-HANDOFF.md` for the active milestone before writing any code.

---

## Milestone Map

| M | Build | Key concept |
|---|-------|-------------|
| M1 | EmployeeService + Dockerfile | Containerization |
| M2 | HolidayService + Docker Compose | Multi-container orchestration |
| M3 | LeaveService (calls Employee + Holiday) | Inter-service communication |
| M4 | YARP Gateway | Single entry point, routing |
| M5 | AuthService + JWT across all services | Token propagation |
| M6 | RabbitMQ + MassTransit events | Async event-driven communication |
| M7 | Database-per-service enforcement | Data isolation |
| M8 | ShiftService + AttendanceService | Expanding a live system |
| M9 | Brownfield: extract PerfinTracker module | Strangler fig pattern |
| M10 | React frontend (minimal) | Frontend-to-microservices integration |

After each milestone: append a summary to `Sprints/MILESTONE-SUMMARIES.md`.

---

## Key Files

| File | Purpose |
|------|---------|
| `SESSION-HANDOFF.md` | Current state, active milestone, next steps |
| `CONTEXT.md` | Full architecture doc — service map, contracts, decisions |
| `Sprints/SPRINT1.md` | Task list for M1 + M2 |
| `Sprints/MILESTONE-SUMMARIES.md` | Permanent learning record — append-only |
| `docker-compose.yml` | Root orchestration file — grows per milestone |
| `services/employee-service/` | First service, reference implementation for all others |

---

## Critical Rules

1. Read `SESSION-HANDOFF.md` before touching any code
2. `Core` project must have zero NuGet dependencies — verify `*.csproj` before committing
3. Never return domain entities from API endpoints — always map to DTOs
4. Never introduce a milestone's infrastructure (Compose, gateway, RabbitMQ) before that milestone
5. Write `SESSION-HANDOFF.md` when session approaches limits OR milestone completes
6. Append to `Sprints/MILESTONE-SUMMARIES.md` when a milestone is complete

---

## DO NOT

- Mix two services in one `.sln`
- Call `DbContext` from a controller or endpoint
- Add NuGet packages to the `Core` project
- Share a database between services
- Reference services by `localhost` inside Docker — use container names
- Skip the milestone summary when completing a milestone
- Jump ahead to a future milestone's infrastructure without completing the current one

---

## Session Handoff Trigger Table

| Condition | Action |
|-----------|--------|
| ~80% session token usage | Write/update `SESSION-HANDOFF.md` immediately |
| ~60% context window usage | Write/update `SESSION-HANDOFF.md` immediately |
| Milestone completed | Write `SESSION-HANDOFF.md` + append to `MILESTONE-SUMMARIES.md` |
| End of any productive session | Write/update `SESSION-HANDOFF.md` |
