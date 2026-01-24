# Teams Management API

A **production‑ready Team Management REST API** built with **ASP.NET Core (.NET 8)** following **Clean / Onion Architecture** principles. The API manages **organizations, teams, users, projects, and tasks** with full lifecycle support, **soft deletion**, and clear separation of concerns.

This project is designed to be **CV‑ready**: clean architecture, scalable structure, and real‑world domain modeling.

---

## 🚀 Key Features

* Clean **Onion Architecture** with strict dependency rules
* Rich domain model (Organizations, Teams, Projects, Tasks, Users)
* **Soft delete & restore** across all aggregates
* Repository + Unit of Work patterns
* DTO-based API contracts (no domain leakage)
* EF Core code-first with migrations
* Designed for extensibility and testability

---

## 🧱 Architecture Overview

This project follows **Onion Architecture**, where **dependencies always point inward** toward the domain. The domain layer is completely independent of frameworks and infrastructure concerns.
---

## 📦 Solution Structure

```text
TeamsManagementProject
│
├── Core
│   └── Domain
│       ├── Entities
│       ├── Enums
│       └── Contracts
│           ├── Repositories
│           ├── Security
│           └── IUnitOfWork
│
├── Application
│   ├── Services
│   ├── Services.Abstractions
│   ├── MappingProfiles
│   └── Extensions
│
├── Infrastructure
│   └── Persistence
│       ├── Data
│       │   ├── Configurations
│       │   ├── Migrations
│       │   └── Seeding
│       ├── Repositories
│       ├── Hash
│       └── AppDbContext
│
├── Shared
│   └── DTOs
│       ├── OrganizationDTOs
│       ├── TeamDTOs
│       ├── ProjectDTOs
│       ├── TaskDTOs
│       └── UserDTOs
│
└── TeamsManagementProject.API
    ├── Controllers
    ├── Program.cs
    └── appsettings.json
````

---

## 🧠 Domain Model

### Core Entities

* **Organization** → owns Users & Teams
* **Team** → belongs to Organization, contains Members & Projects
* **User** → belongs to Organization, can join multiple Teams
* **Project** → belongs to a Team, contains Tasks
* **Task** → belongs to Project, optionally assigned to a User

### Base Entity

All main entities inherit:

* `Id`
* `CreatedAt`
* `UpdatedAt`
* `IsDeleted` (soft delete)

---

## 🔁 Soft Delete Strategy

* `PATCH /soft-delete` → sets `IsDeleted = true`
* `PATCH /restore` → restores entity
* `GET /deleted` → fetch soft‑deleted records

✔ Prevents data loss
✔ Enables auditing and recovery
✔ Real‑world enterprise pattern

---

## 🔌 API Endpoints Overview

### 🧩 Tasks

* `GET /tasks/{id}`
* `POST /tasks`
* `PUT /tasks/{id}`
* `DELETE /tasks/{id}` (hard delete)
* `PATCH /tasks/{id}/soft-delete`
* `PATCH /tasks/{id}/restore`
* `GET /tasks/deleted`
* `PATCH /tasks/{id}/status`
* `PATCH /tasks/{id}/assign/{userId}`

---

### 🏢 Organizations

* `GET /organizations`
* `GET /organizations/{id}`
* `POST /organizations`
* `PUT /organizations/{id}`
* `DELETE /organizations/{id}`
* `PATCH /organizations/{id}/soft-delete`
* `PATCH /organizations/{id}/restore`
* `GET /organizations/deleted`
* `GET /organizations/statistics`

---

### 📁 Projects

* `GET /projects/{id}`
* `POST /projects`
* `PUT /projects/{id}`
* `PATCH /projects/{id}/status`
* `DELETE /projects/{id}`
* `PATCH /projects/{id}/soft-delete`
* `PATCH /projects/{id}/restore`
* `GET /projects/deleted`
* `GET /projects/by-team/{teamId}`
* `GET /projects/by-organization/{orgId}`

---

### 👥 Teams

* `GET /teams/{id}`
* `POST /teams`
* `PUT /teams/{id}`
* `DELETE /teams/{id}`
* `PATCH /teams/{id}/soft-delete`
* `PATCH /teams/{id}/restore`
* `GET /teams/deleted`
* `GET /teams/by-organization/{orgId}`
* `GET /teams/by-user/{userId}`

#### Team Membership

* `POST /teams/{teamId}/users/{userId}`
* `DELETE /teams/{teamId}/users/{userId}`
* `GET /teams/{teamId}/users/{userId}/exists`

---

### 👤 Users

* `GET /users/{id}`
* `GET /users/by-email`
* `POST /users`
* `PUT /users/{id}`
* `DELETE /users/{id}`
* `PATCH /users/{id}/soft-delete`
* `PATCH /users/{id}/restore`
* `GET /users/deleted`

---

## 🛠️ Tech Stack

* **.NET 8 / ASP.NET Core Web API**
* **Entity Framework Core**
* **AutoMapper**
* **Repository & Unit of Work patterns**
* **SQL Server (configurable)**

---

## ⚙️ Running the Project

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Update the connection string in `appsettings.json` before running migrations.

---

## 🔮 Future Improvements

* Authentication & authorization (JWT, roles)
* Pagination, filtering, and sorting
* Global exception handling middleware
* Unit & integration testing

---

## 👨‍💻 Author

**Marwan** – Software Engineer (.NET)
