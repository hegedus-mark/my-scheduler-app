# Backend Architecture Documentation

## Overview

This documentation provides an overview of the architectural patterns and design principles used in our backend application. The application is built using .NET Core, following Domain-Driven Design (DDD) principles and implementing the CQRS pattern with Mediator for handling commands and queries.

## Core Architectural Patterns

### Domain-Driven Design (DDD)

We follow DDD principles to maintain a clear separation of concerns and ensure our domain logic is properly encapsulated. Key concepts include:

- **Aggregates**: Root entities that maintain consistency boundaries (e.g., `TaskItem`, `CalendarDay`)
- **Value Objects**: Immutable objects representing concepts with no identity (e.g., `TimeSlot`, `DateRange`)
- **Domain Events**: Events that represent significant changes in the domain (e.g., `TaskScheduledEvent`)

### CQRS (Command Query Responsibility Segregation)

We separate read and write operations using the CQRS pattern:

- **Commands**: Represent intentions to change the system state (e.g., `CreateTaskCommand`)
- **Queries**: Request data from the system (e.g., `GetAllTasksQuery`)
- **Benefits**: Better scalability, separation of concerns, and optimization potential

📚 Learn more: [CQRS Pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)

### Mediator Pattern

We use the mediator pattern to:

- Decouple components
- Handle commands and queries
- Centralize cross-cutting concerns

## Project Structure

```
src/
  ├── Api/                 # API Controllers and configuration
  ├── Application/        # Application services, commands, and queries
  │   ├── Calendar/       # Calendar-related features
  │   ├── Scheduling/     # Scheduling-related features
  │   └── Shared/         # Shared components and interfaces
  ├── Domain/            # Domain models and business logic
  └── Infrastructure/    # Data access, external services
```

## Getting Started

See specific documentation for each pattern:

- [Messaging Pattern](./patterns/messaging.md)
