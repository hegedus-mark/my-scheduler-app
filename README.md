# My Scheduler App
A full-stack scheduling application that helps users efficiently plan their time by automatically organizing tasks based on priorities, deadlines, and available time slots.

## About

This project is a web-based evolution of the scheduling algorithm originally created by Andrew Roe in [quick-sched](https://github.com/AndrewRoe34/quick-sched). While the original project was a Java CLI application, this implementation will provide a full web interface and additional features using modern development practices.

## Tech Stack

### Backend
- ASP.NET Core Web API
- Entity Framework Core
- Domain driven design, CQRS pattern, Clean (Onion) Architecture

### Frontend
- Angular

## Features

- Automatic task scheduling based on:
  - Task priority
  - Deadlines
  - Duration
  - Available time slots
- Smart conflict resolution
- [More soon]

## Project Status

🚧 This project is currently under development. 


## Project Architecture

This project follows Clean Architecture principles with Domain-Driven Design (DDD) concepts, organized into bounded contexts. The architecture is structured into several distinct layers, each with a specific responsibility:

### Layers

#### API Layer
- Entry point for all external requests
- Handles HTTP requests/responses
- Manages routing and basic request validation
- Separated by feature contexts (Calendar, Scheduling)

#### Application Layer
- Orchestrates flow between API and Domain layers
- Contains application-specific business rules
- Implements use cases and application services
- Manages DTOs and interface contracts

#### Domain Layer
- Heart of the business logic
- Contains domain entities, value objects, and domain events
- Implements core business rules and domain logic
- Completely independent of other layers

#### Infrastructure Layer
- Implements persistence and external service integration
- Contains concrete implementations of repository interfaces
- Manages database context and migrations
- Handles technical concerns (logging, caching, etc.)

## Bounded Contexts

The application is divided into two main bounded contexts:

#### Calendar Context
- Manages calendar-related operations
- Handles working days, time slots, and calendar items
- Manages calendar-specific business rules

#### Scheduling Context
- Handles task scheduling and management
- Implements scheduling algorithms and priority handling
- Manages task-related operations and states

Each context maintains its own set of models, services, and repositories across all layers, ensuring proper separation of concerns and maintainability.

![Server-architecture](https://github.com/user-attachments/assets/7fb3f142-351a-4189-b457-e0474d59c4af)
