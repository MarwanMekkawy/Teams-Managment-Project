## ✅ Project Status

> **Feature Complete**  
> This project has reached a stable and feature-complete state.  
> All core functionality is implemented.  
> Minor improvements and optimizations may be introduced in future updates.

## API Base URL

The base URL for all API requests is:  

[http://teamsmanage.runasp.net/api/v1/](http://teamsmanage.runasp.net/api/v1/)

# Teams Management API
![Status](https://img.shields.io/badge/status-feature%20complete-success)
![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![Architecture](https://img.shields.io/badge/architecture-Clean%20Onion-informational)

A **production‑ready Team Management REST API** built with **ASP.NET Core (.NET 8)** following **Clean / Onion Architecture** principles. The API manages **organizations, teams, users, projects, and tasks** with full lifecycle support, **soft deletion**, and clear separation of concerns.

This project is designed to be **CV‑ready**: clean architecture, scalable structure, and real‑world domain modeling.

---

## 🚀 Key Features

* Clean **Onion Architecture** with strict dependency rules
* Rich domain model (Organizations, Teams, Projects, Tasks, Users)
* **Soft delete & restore** across all aggregates
* Repository + Unit of Work patterns
* DTO-based API contracts 
* EF Core code-first with migrations
* Designed for extensibility and testability

---

## 🧱 Architecture Overview

This project follows **Onion Architecture**, where **dependencies always point inward** toward the domain. The domain layer is completely independent of frameworks and infrastructure concerns.
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

Soft deletion is implemented consistently across all aggregates.

### Behavior

- Soft delete sets `IsDeleted = true`
- Restore clears `IsDeleted`
- Deleted records are excluded from normal queries
- Dedicated endpoints expose deleted data

### Endpoints Pattern

- `PATCH /{entity}/{id}/soft-delete`
- `PATCH /{entity}/{id}/restore`
- `GET /{entity}/deleted`

---

## 🔌 API Endpoints

### 🏢 Organizations

- `GET /organizations`
- `GET /organizations/{id}`
- `POST /organizations`
- `PUT /organizations/{id}`
- `DELETE /organizations/{id}`
- `PATCH /organizations/{id}/soft-delete`
- `PATCH /organizations/{id}/restore`
- `GET /organizations/deleted`
- `GET /organizations/statistics`

---

### 👥 Teams

- `GET /teams/{id}`
- `POST /teams`
- `PUT /teams/{id}`
- `DELETE /teams/{id}`
- `PATCH /teams/{id}/soft-delete`
- `PATCH /teams/{id}/restore`
- `GET /teams/deleted`
- `GET /teams/by-organization/{orgId}`
- `GET /teams/by-user/{userId}`

#### Team Membership

- `POST /teams/{teamId}/users/{userId}`
- `DELETE /teams/{teamId}/users/{userId}`
- `GET /teams/{teamId}/users/{userId}/exists`

---

### 👤 Users

- `GET /users/{id}`
- `GET /users/by-email`
- `POST /users`
- `PUT /users/{id}`
- `DELETE /users/{id}`
- `PATCH /users/{id}/soft-delete`
- `PATCH /users/{id}/restore`
- `GET /users/deleted`

---

### 📁 Projects

- `GET /projects/{id}`
- `POST /projects`
- `PUT /projects/{id}`
- `PATCH /projects/{id}/status`
- `DELETE /projects/{id}`
- `PATCH /projects/{id}/soft-delete`
- `PATCH /projects/{id}/restore`
- `GET /projects/deleted`
- `GET /projects/by-team/{teamId}`
- `GET /projects/by-organization/{orgId}`

---

### 🧩 Tasks

- `GET /tasks/{id}`
- `POST /tasks`
- `PUT /tasks/{id}`
- `DELETE /tasks/{id}` (Hard Delete)
- `PATCH /tasks/{id}/soft-delete`
- `PATCH /tasks/{id}/restore`
- `GET /tasks/deleted`
- `PATCH /tasks/{id}/status`
- `PATCH /tasks/{id}/assign/{userId}`

---

## 🛠️ Tech Stack

- **ASP.NET Core Web API (.NET 8)**
- **Entity Framework Core**
- **SQL Server**
- **AutoMapper**
- **Repository + Unit of Work**
- **DTO-based API contracts**

---

## ⚙️ Running the Project

```bash
dotnet restore
dotnet ef database update
dotnet run
---

## 👨‍💻 Author

**Marwan** – Software Engineer (.NET)

