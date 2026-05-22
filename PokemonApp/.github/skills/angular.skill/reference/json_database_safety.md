# Skill: JSON Database Safety & Concurrency

This document outlines the safety guidelines, locking mechanisms, and file system management rules when using a local JSON file-based database context like `JsonDatabaseContext.cs` in C#.

---

## 1. Concurrency Management & Thread Safety

- **The Lock Object Boundary:**
  - Because multiple web API requests access the database singleton concurrently, all read and write operations targeting the shared internal lists (`_pokemons` and `_items`) must be synchronized.
  - Implement a dedicated lock object: `private readonly object _lock = new();`.
- **Minimized Lock Scope:**
  - Keep the code inside `lock (_lock) { ... }` blocks as short and fast as possible. Never run asynchronous awaits (like HTTP requests or slow file writes) inside a lock block, as this can block the thread pool and lead to deadlocks.
- **Defensive Copying on Read:**
  - To prevent external code from modifying the database collections bypass-context, always return a copy of the list rather than the original list reference:
    ```csharp
    public List<PokemonDetailDto> Pokemons
    {
        get { lock (_lock) { return _pokemons.ToList(); } }
    }
    ```

---

## 2. Robust File System Serialization

- **Safe Directory Initializations:**
  - Ensure parent directories exist before writing to files. Use `Directory.CreateDirectory(...)` which gracefully returns if the target folder already exists.
- **Handling IO Failures:**
  - Wrap database read/write actions in `try-catch` blocks to capture issues like access permissions, file locks, or disk-full states, logging the failure instead of crashing the process.
- **Atomic File Writing (Durability):**
  - For critical production databases, avoid writing directly to the active database file (which can leave the file empty or corrupted if the application terminates mid-write).
  - **Preferred Pattern:** Write to a temp file first, then replace the original file atomically:
    ```csharp
    var tempPath = _filePath + ".tmp";
    File.WriteAllText(tempPath, json);
    File.Move(tempPath, _filePath, overwrite: true);
    ```

---

## 3. Serialization Standards (System.Text.Json)

- **Write Indented Output:**
  - For debugging and version-control tracking, write JSON databases in indented formatting:
    ```csharp
    new JsonSerializerOptions { WriteIndented = true }
    ```
- **Null Reference Protections:**
  - When deserializing, check for null results before setting internal collections. If deserialization fails or returns null, fallback to initializing empty lists (`new()`) instead of exposing null references.
- **Maintaining Order consistency:**
  - Ensure items are sorted (e.g. by ID) prior to serialization. This keeps diff commits clean when database changes are tracked in version control systems like Git.
