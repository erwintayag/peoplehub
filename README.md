# PeopleHub

Microservices learning project — HR domain built on .NET 10, Docker, and Clean Architecture.

## What it does

A hands-on microservices sandbox using an HR system as the domain. Progressively builds
from a single containerized service to a full event-driven microservices architecture across 10 milestones.

## Tech Stack

| Layer | Technology |
|-------|------------|
| Services | .NET 10 Minimal API |
| Architecture | Clean Architecture (Core / Application / Infrastructure / API per service) |
| Database | SQLite (dev) → PostgreSQL (M7+) |
| Containers | Docker + Docker Compose |
| Gateway | YARP |
| Messaging | RabbitMQ + MassTransit (M6+) |
| Frontend | React 19 + Vite + TypeScript (M10) |

## Service Map

| Service | Port | Status |
|---------|------|--------|
| Gateway (YARP) | :5000 | M4 |
| AuthService | :5001 | M5 |
| EmployeeService | :5002 | M1 🔄 |
| HolidayService | :5003 | M2 |
| LeaveService | :5004 | M3 |

## Setup

### Prerequisites
- .NET 10 SDK
- Docker Desktop

### Run (M2+ — Docker Compose)

```bash
docker compose up
```

All services start; gateway available at `http://localhost:5000`.
Swagger UI per service available at their individual ports.

### Run (M1 — single service)

```bash
cd services/employee-service/src/PeopleHub.Employee.API
dotnet run
# Swagger: http://localhost:5002/swagger
```

### Build Docker image (M1)

```bash
docker build -t peoplehub-employee ./services/employee-service
docker run -p 5002:8080 peoplehub-employee
```

## Learning Progress

See `Sprints/MILESTONE-SUMMARIES.md` for completed milestone write-ups.
See `SESSION-HANDOFF.md` for current active state.
