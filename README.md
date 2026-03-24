# 🎫 Ticket System API

REST API for managing support tickets built with **.NET**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

---

## 🚀 What I built

This project simulates a real-world ticket management system where:

* Tickets can be created and managed
* Business rules are enforced at the domain level
* Reopen policies are configurable
* Caching is implemented at the application layer

---

## 🧠 Architecture

This project follows:

```
API → Application → Domain → Infrastructure
```

### 🔹 Domain

* Aggregate Root: Ticket
* Value Objects: TicketStatus, TicketPriority
* Domain Events
* Business Rules (Invariants)

### 🔹 Application

* UseCases (CreateTicket, ReopenTicket, GetTicketById)
* DTOs
* Caching (MemoryCache)

### 🔹 Infrastructure

* Entity Framework Core
* SQL Server
* Repository Pattern

---

## ⚡ Tech Stack

* .NET
* Entity Framework Core
* SQL Server
* xUnit + Moq
* Swagger

---

## 🧪 Example Request

POST /api/tickets

```json
{
  "title": "Printer issue",
  "description": "Printer is not working",
  "priority": "Medium"
}
```

---

## 🧠 Key Concepts Practiced

* Clean Architecture
* Domain-Driven Design (DDD)
* CQRS (basic separation)
* Caching strategies
* Repository pattern
* Async / Await

---

## 📈 Next Steps

* Redis Cache
* Integration Tests
* Docker
* CI/CD

---

## 👩‍💻 Author

Lorena Chagoya
