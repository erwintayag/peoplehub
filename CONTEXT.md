# PeopleHub — Context

## Purpose & Scope

Microservices learning project. Domain: HR system.
Goal: build hands-on confidence working with microservices in both greenfield and brownfield contexts.
Each milestone introduces one new architectural concept through working code.

This is an educational project — not production software. Architectural choices are deliberate
teaching vehicles, not necessarily what you'd pick for a real system on day one.

---

## Tech Stack

| Layer | Technology | Rationale |
|-------|------------|-----------|
| Services | .NET 10 Minimal API | Familiar stack — removes language friction, focuses learning on microservices patterns |
| ORM | EF Core + SQLite (dev) | Familiar from other projects; SQLite removes DB setup overhead for early milestones |
| Containers | Docker + Docker Compose | Industry standard; prerequisite for microservices at any scale |
| API Gateway | YARP (Yet Another Reverse Proxy) | Microsoft-native, .NET-friendly, minimal config overhead |
| Async messaging | RabbitMQ + MassTransit | Introduced at M6; industry-standard message broker with a .NET-friendly abstraction |
| Architecture | Clean Architecture per service | Enforces OOP + SOLID; consistent with all other projects/ subprojects |

---

## Architecture Overview

Pattern: Microservices — each service owns its domain, data, and deployment unit.
Services are independently buildable, runnable, and deployable.

```
┌─────────────────────────────────────────────────────┐
│  Client (Swagger / Postman → M10: React frontend)  │
└────────────────────┬────────────────────────────────┘
                     │ :5000
          ┌──────────▼──────────┐
          │   Gateway (YARP)    │  routes by path prefix
          └──┬────┬────┬────────┘
         :5001 :5002 :5003 :5004
          │    │    │    │
     Auth  Emp  Hol  Leave
```

### Service Map

| Service | Port | Responsibility |
|---------|------|----------------|
| Gateway (YARP) | :5000 | Single entry point; routes to services by path prefix |
| AuthService | :5001 | JWT issuance, user accounts, role-based access |
| EmployeeService | :5002 | Employee profiles, org structure, department data |
| HolidayService | :5003 | Public holiday calendar; answers "is this date a holiday?" |
| LeaveService | :5004 | Leave requests, approvals, balance tracking |

### Communication model

| Milestone range | Pattern | Why |
|----------------|---------|-----|
| M1–M5 | Sync HTTP (REST) | Simpler mental model; learn the pain of synchronous dependencies first |
| M6+ | Async events via RabbitMQ | Introduces loose coupling; services stop calling each other directly |

### Dependency graph (sync phase, M1–M5)

```
LeaveService ──HTTP──► EmployeeService  (is this employee valid?)
LeaveService ──HTTP──► HolidayService   (does leave overlap a holiday?)
All services ──JWT──►  AuthService      (token validation via shared signing key — no direct HTTP call)
```

---

## Data Model (per-service, no shared DB)

### EmployeeService
```
Employees
├── Id (GUID PK)
├── FirstName, LastName
├── Email (unique)
├── Department, JobTitle
├── HireDate
├── IsActive
└── CreatedAt, UpdatedAt
```

### HolidayService
```
Holidays
├── Id (GUID PK)
├── Name
├── Date (DateOnly, unique per year)
├── IsRecurring (true = applies every year)
└── CreatedAt
```

### LeaveService
```
LeaveRequests
├── Id (GUID PK)
├── EmployeeId (reference — NOT FK to Employees table; that lives in EmployeeService)
├── StartDate, EndDate
├── LeaveType (Annual, Sick, Unpaid)
├── Status (Pending, Approved, Rejected)
├── RejectionReason
└── CreatedAt, UpdatedAt
```

### AuthService
```
Users
├── Id (GUID PK)
├── Email (unique)
├── PasswordHash
├── Role (Admin, Manager, Employee)
└── CreatedAt
```

---

## Inter-Service Contracts

### EmployeeService → consumed by LeaveService

```
GET /api/v1/employees/{id}
Response: { id, firstName, lastName, email, department, isActive }
LeaveService calls this to verify the employee exists before accepting a leave request.
```

### HolidayService → consumed by LeaveService

```
GET /api/v1/holidays/check?startDate=YYYY-MM-DD&endDate=YYYY-MM-DD
Response: { hasHoliday: bool, holidays: [{ date, name }] }
LeaveService calls this to check if a leave request overlaps a public holiday.
```

### AuthService → consumed by all services

Services do NOT call AuthService directly. They validate JWT tokens locally using the shared signing key
configured in each service's appsettings.json. The Gateway handles auth enforcement at the routing layer (M5).

---

## Dev Rules & Invariants

**R01 — No shared database.** Each service has its own SQLite file. A service never reads or writes
another service's database, even in dev. If data is needed from another service, call its API.

**R02 — No `localhost` in Docker.** Services reference each other by container name
(e.g., `http://employee-service:8080`). `localhost` resolves to the container itself, not siblings.

**R03 — Core project = zero NuGet deps.** If `PeopleHub.*.Core.csproj` has any `<PackageReference>`,
it is a Clean Architecture violation. Fix before committing.

**R04 — DTOs at API boundary.** Domain entities (Core models) are never serialised in HTTP responses.
Application layer DTOs are the only types that cross the service boundary.

**R05 — Milestone sequencing.** Do not introduce M(n+1) infrastructure while M(n) is incomplete.
No Docker Compose before M2, no Gateway before M4, no RabbitMQ before M6.

**R06 — Constructor injection everywhere.** No `new ConcreteService()` in Application or Core.
All dependencies injected via constructor; registered in `ServiceCollectionExtensions.cs`.

**R07 — Health check required.** Every service exposes `GET /health` → 200 OK before it is
considered "done" for its milestone. Docker Compose `healthcheck:` uses this endpoint.

---

## Milestone Progress

| M | Status | Completed |
|---|--------|-----------|
| M1 | 🔄 In progress | — |
| M2–M10 | ⏳ Pending | — |

See `Sprints/MILESTONE-SUMMARIES.md` for completed milestone write-ups.

---

## Known Limitations

- SQLite is dev-only — does not support concurrent writes well; replaced with PostgreSQL at M7
- No distributed tracing (Jaeger/Zipkin) — added as an optional stretch goal after M5
- No Kubernetes — Docker Compose only; K8s is a post-M10 extension if desired
- JWT signing key shared via appsettings — acceptable for learning; use Azure Key Vault in real projects
