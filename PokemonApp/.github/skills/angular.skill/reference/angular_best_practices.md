# Skill: Angular Best Practices

This document outlines the coding standards, patterns, and best practices for developing and maintaining the Angular SPA (`ClientApp`) frontend of the Pokémon Application.

---

## 1. Component Architecture & Separation of Concerns

- **Smart vs. Presentational (Dumb) Components:**
  - **Smart Components** (e.g. `PokemonComponent`, `ObjectsComponent`): Manage state, handle routing, trigger side-effects, and make HTTP/service requests.
  - **Presentational Components** (e.g. NavMenu, or future Card/Modal components): Receive data via `@Input()` and emit events via `@Output()`. They should be highly reusable and have zero knowledge of API endpoints or services.
- **Single Responsibility Principle (SRP):** Keep component classes small. A component's main job is to display data and bind events. Heavy logic should be outsourced to services.

---

## 2. Safe State Management & RxJS Patterns

- **Preventing Memory Leaks:**
  - When subscribing to observables manually in component classes, always clean them up.
  - **Prefer Async Pipe:** Let Angular handle subscription/unsubscription automatically inside templates using `*ngIf="data$ | async as data"`.
  - **Alternative (Manual):** If manual subscription is necessary, use `takeUntil` or the RxJS `Subscription` array to unsubscribe during the `ngOnDestroy` lifecycle hook:
    ```typescript
    private destroy$ = new Subject<void>();

    ngOnInit() {
      this.myService.getData()
        .pipe(takeUntil(this.destroy$))
        .subscribe(val => this.value = val);
    }

    ngOnDestroy() {
      this.destroy$.next();
      this.destroy$.complete();
    }
    ```
- **Declarative Streams:** Combine RxJS operations using operators like `switchMap`, `map`, and `catchError` instead of nesting `.subscribe()` inside other `.subscribe()` callbacks.

---

## 3. Typed API Interaction

- **Strong Typing:** Always define typescript interfaces for all API response schemas. Never use the `any` keyword.
  ```typescript
  interface PokemonResponse {
    totalCount: number;
    page: number;
    pageSize: number;
    items: Pokemon[];
  }
  ```
- **Encapsulate HTTP Requests:** Move all direct `HttpClient` calls out of components and place them inside designated services (e.g., `PokemonService`, `ItemService`). This allows components to remain logic-agnostic and simplifies unit testing.

---

## 4. UI, Styling & Accessibility (Tailwind CSS)

- **Consistent Styling System:**
  - Use custom tailwind colors (`pokeRed`, `pokeBlack`, `pokeYellow`) defined in the Tailwind configuration to ensure brand coherence.
  - Avoid raw color values (like `bg-red-500` or `#ff0000`). Prefer matching HSL values or the custom system colors.
- **Responsive Layouts:**
  - Design with a mobile-first approach using Tailwind's breakpoint prefixes (e.g., `grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4`).
- **Interactive UI Feedback:**
  - Always implement state states (e.g., `isLoading`, `isSaving`) to disable buttons, display loading skeletons, or indicate activity to the user.
