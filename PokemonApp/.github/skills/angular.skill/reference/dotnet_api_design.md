# Skill: .NET API Design & Best Practices

This document outlines the coding standards, patterns, and best practices for developing and maintaining the ASP.NET Core (`net10.0`) Web API backend of the Pokémon Application.

---

## 1. RESTful API Routing and Action Conventions

- **Clear and Consistent Routing:**
  - Route templates must use resource-oriented, plural nouns. Avoid verbs in the route paths:
    - **Correct:** `GET /api/pokemon` and `POST /api/pokemon`
    - **Incorrect:** `GET /api/getPokemon` or `POST /api/createPokemon`
- **Appropriate HTTP Verbs:**
  - `GET`: Read lists or specific detail resources (must remain safe and side-effect free).
  - `POST`: Create a new resource.
  - `PUT`: Replace or update an existing resource.
  - `DELETE`: Delete a resource.
- **Explicit Status Codes:**
  - Return context-correct HTTP response statuses via action results:
    - `200 OK` for successful read/update operations.
    - `201 Created` for successful resource creations (include the location URI if applicable).
    - `400 BadRequest` if incoming payload validation fails.
    - `404 NotFound` if the requested ID does not exist in the store.

---

## 2. Controller & Service separation (Clean Architecture)

- **Slim Controllers:**
  - Keep controllers lightweight. A controller's only responsibility is HTTP request parsing, routing, and returning the response.
  - Do not write database-access code or calculation logic directly inside controller action methods.
- **Dependency Injection (DI) Lifetimes:**
  - Register services in `Program.cs` with the appropriate lifetime scope:
    - **Singleton:** Use when state must be shared globally (e.g. database context `JsonDatabaseContext.cs` which manages class-level lock boundaries).
    - **Scoped:** Use for request-bounded services (e.g. business logic services `IPokemonService`).
    - **Transient:** Use for lightweight, stateless helper classes.
- **Interface Segregation:**
  - Controllers should depend on service interfaces (`IPokemonService`), never direct concrete classes. This supports dependency decoupling and mock testing.

---

## 3. Data Safety: DTOs & Model Validation

- **Never Expose Internal Entities:**
  - Do not accept or return database context models directly in controller actions. Use DTOs (Data Transfer Objects) instead to control exactly what is sent/received.
- **Model Validation:**
  - Use data annotations (`[Required]`, `[StringLength]`, `[Range]`) in DTO definitions to validate user inputs automatically.
  - Check `ModelState.IsValid` before initiating database alterations.

---

## 4. Async/Await & Concurrency Guidelines

- **Asynchronous Execution:**
  - Use `async` and `await` consistently from controllers down to the data access layer to prevent thread pool starvation.
  - Avoid blocking synchronous calls (like `.Result` or `.Wait()`) on asynchronous actions.
- **Async Void Prevention:**
  - Never use `async void` except in event handlers. For asynchronous tasks that do not return data, return `Task`.
- **Thread Safety in Caching & Contexts:**
  - Since singletons can be accessed concurrently by multiple requests, ensure all shared fields are protected with lock mechanisms or thread-safe collections.
- **Safe Background Tasks:**
  - Fire background startup operations (like seeding database files) in a separate thread context (e.g. `Task.Run(...)`) and handle exceptions locally to prevent server startup failures.
