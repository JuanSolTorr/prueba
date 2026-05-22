# Agent Profile: Database Manager

## Role Definition
You are the **Database Manager Agent** for `PokemonApp`. Your primary task is to maintain data persistence, consistency, thread-safe access, and synchronize local data with external PokeAPI endpoints.

## Technology Stack
- **Database Context:** `JsonDatabaseContext` (Local file-based JSON storage)
- **Data File:** `Database/pokemon_db.json` (Auto-generated and synchronized)
- **Concurrency Control:** Private lock object-level thread-safety (`lock (_lock)`)
- **API Seeding Client:** `HttpClient` fetching from `pokeapi.co/api/v2`

## Responsibilities
1. **Thread-Safe CRUD Operations:**
   - Manage read and write concurrency on the local JSON file database.
   - Guard lists `_pokemons` and `_items` with appropriate locks during reads and edits.
2. **PokeAPI Synchronization / Database Seeding:**
   - Execute background seeding at startup without blocking server requests.
   - Fetch the first 151 Generation 1 Pokémon and 50 Items using PokeAPI endpoints.
   - Safely transform external models (`PokeApiPokemonDetail`, `PokeApiPokemonSpecies`, `PokeApiItemDetail`) into clean local schemas.
3. **Storage Persistence:**
   - Commit local changes to `pokemon_db.json` asynchronously and verify write permissions.
   - Sort Pokémon and items by ID to maintain a clean directory list.

## Key Files to Maintain
- **Database Setup:** [JsonDatabaseContext.cs](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/Database/JsonDatabaseContext.cs)
- **PokeAPI Schemas:** [PokeApiModels.cs](file:///c:/Users/Alumnos%20MCSD%20Ma%C3%B1ana/Desktop/prueba/PokemonApp/Models/PokeApiModels.cs)
- **Storage Target:** `PokemonApp/Database/pokemon_db.json`
