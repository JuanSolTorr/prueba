# Pokémon App - Project Status & Context

This document serves as the main context map and documentation for the `PokemonApp` project. It summarizes the current state, tech stack, agent fleet organization, and future task list.

---

## 🛠️ Technology Stack Review

The application is built on a modern hybrid stack consisting of a C# ASP.NET Core backend and an Angular frontend:

| Component | Technology | Version | Key Details |
| :--- | :--- | :--- | :--- |
| **Backend** | .NET (ASP.NET Core) | `net10.0` | API Controllers, SpaProxy configuration |
| **Frontend** | Angular | `^15.2.8` | Component-based, SPA routing, RxJS streams |
| **Styling** | Tailwind CSS | `^3.4.19` | Premium dark mode theme, glassmorphic panels, custom animations |
| **Database** | File-based JSON DB | Custom context | `JsonDatabaseContext` storing in `Database/pokemon_db.json` |
| **External API** | PokeAPI | REST | Synchronized seeding at startup |

---

## 🏗️ Architecture & Features

The project is structured logically across two layers:

### 1. Backend (`PokemonApp/`)
* **Controllers:**
  * `PokemonController.cs`: Exposes endpoints for paginated search, retrieval of detailed specs, creation of custom Pokémon, and deleting/liberating Pokémon.
  * `ItemController.cs`: Exposes endpoints for paginated search and category selection of items.
* **Services & Context:**
  * `JsonDatabaseContext.cs`: Seeds data from PokeAPI (first 151 Generation 1 Pokémon and 50 Items) on background initialization, keeping operations non-blocking and safe via class-level locks.
  * DTO models ensure proper serialization mapping using `System.Text.Json`.

### 2. Frontend (`PokemonApp/ClientApp/`)
* **Views:**
  * **Pokémon Center:** A dashboard showing the catalog of Pokémon with infinite loading, filter by type, full detail modal with visual stat charts, and CRUD creation capabilities.
  * **Objects Center:** Lists available items with custom category tags, name search, and detail drawers.
* **Aesthetics:** Custom animations (spin, shake, beam glowing effects), scrollbars, and Outfit font for high visual appeal.

---

## 👥 AI Agent Fleet Configuration

To optimize development, tasks are divided among five specialized agent personas:

1. **Orchestrator Agent ([orchestrator.md](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/agents/orchestrator.md)):** Coordinates routing, architectural division, and overall integration.
2. **Frontend Developer ([frontend_developer.md](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/agents/frontend_developer.md)):** Focuses on Angular logic, responsive designs, Tailwind configuration, and smooth animations.
3. **Backend Developer ([backend_developer.md](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/agents/backend_developer.md)):** Manages controllers, route schemas, services structure, and Web API rules.
4. **Database Manager ([database_manager.md](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/agents/database_manager.md)):** Ensures database file read/write performance, thread-safety, and external PokeAPI synchronization.
5. **QA & Tester ([qa_tester.md](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/agents/qa_tester.md)):** Validates visual layouts, designs unit tests, mocks API endpoints, and prevents regressions.

---

## 📋 Future Roadmap & Task Lists

Here are the future tasks planned to improve the application:

### Short-Term (Immediate Enhancements)
- [ ] **Error Handling & Toast Notifications:** Add stylized Tailwind toast notifications for CRUD actions (e.g. success on creation, errors on database write failure).
- [ ] **Pagination Controls:** Replace basic "Load More" with an option for dynamic paging controls.
- [ ] **Validation on Creation:** Add client-side validation to the Pokemon Creation form (e.g. limit weight/height, type safety).
- [ ] **Visual Testing Setup:** Setup basic Jasmine component specs for `pokemon.component.ts` to ensure coverage of modal states.

### Medium-Term (New Features)
- [ ] **Pokemon Edit Capabilities:** Implement a full Update/Edit form in the detail modal.
- [ ] **Favorites / Team Builder:** Allow users to flag favorites or assemble a custom team of 6 Pokémon saved in the JSON database context.
- [ ] **Custom Item Creation:** Extend `ItemController` and `objects.component.html` to support adding custom items.

### Long-Term (Infrastructure)
- [ ] **SQLite / Entity Framework Integration:** Migrate `JsonDatabaseContext` to an SQLite database context using EF Core for relational storage.
- [ ] **Dockerization:** Create Dockerfiles to run the API and ClientApp inside containerized pipelines.
