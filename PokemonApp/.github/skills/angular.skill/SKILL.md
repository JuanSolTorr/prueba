# Skill: Angular Reference and Integration Guide

This guide establishes the direct mappings and code-level references for the Angular SPA application located in [ClientApp](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/). Use these references as concrete context guides when modifying client-side views.

---

## 🧭 Routing and Module Registration

The entry configuration for components and routing paths resides in [AppModule](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/app.module.ts#L34). 
- **Routes Configuration:** Defined in [app.module.ts:L25-L29](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/app.module.ts#L25-L29).
  - Main Catalog Route: `/pokemons` mapping to [PokemonComponent](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/pokemon/pokemon.component.ts#L42).
  - Item Center Route: `/items` mapping to [ObjectsComponent](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/objects/objects.component.ts#L25).

---

## 🐉 Pokémon Catalog Component

The Pokémon center manages listing, detail drawers, creation modals, and deletion protocols.

### 1. Data Retrieval and Filtering
- The main data load is executed inside [loadPokemons](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/pokemon/pokemon.component.ts#L96-L121) using an HTTP GET query string:
  ```typescript
  const url = `/api/pokemon?search=${encodeURIComponent(this.searchText)}&type=${this.selectedType}&page=${this.currentPage}&pageSize=${this.pageSize}`;
  ```
- Trigger bindings are implemented in [pokemon.component.html](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/pokemon/pokemon.component.html) using text filters and type select badges.

### 2. Creation Modals (CRUD)
- Modal status flags `isCreateOpen` and `newPokemon` data structure are initialized in [pokemon.component.ts:L57-L58](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/pokemon/pokemon.component.ts#L57-L58).
- The submission to the ASP.NET Core API is handled in [submitCreate](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/pokemon/pokemon.component.ts#L182-L227), which creates a JSON payload matching the expected backend model schema.

### 3. Deletion (Liberate Pokemon)
- Liberating/deleting a Pokémon from the database file is executed via [deletePokemon](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/pokemon/pokemon.component.ts#L229-L245), sending a `DELETE` request to `/api/pokemon/{id}`.

---

## 🎒 Items Catalog Component

The items catalog handles listing, category tags, search, and detail drawers.

- **Class Definition:** [ObjectsComponent](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/objects/objects.component.ts#L25).
- **Listing Action:** Done in [loadItems](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/objects/objects.component.ts#L44-L70), pulling categories and items from `/api/items`.
- **View Binding:** Bound inside [objects.component.html](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/app/objects/objects.component.html).

---

## 🎨 Theme & Visual Reference Guide

- **Tailwind Extension Palette:** Custom theme extension color properties are configured in [tailwind.config.js:L8-L26](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/tailwind.config.js#L8-L26).
- **Core CSS Animations:** Standard CSS animation classes like `.animate-pokeball-spin`, `.animate-float`, `.glass-card`, and custom type backgrounds are defined in [styles.css:L59-L146](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/ClientApp/src/styles.css#L59-L146).
