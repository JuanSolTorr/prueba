using System.Text.Json;
using PokemonApp.Models;

namespace PokemonApp.Database
{
    public class DatabaseStore
    {
        public List<PokemonDetailDto> Pokemons { get; set; } = new();
        public List<ItemDto> Items { get; set; } = new();
    }

    public class JsonDatabaseContext
    {
        private readonly string _filePath;
        private readonly object _lock = new();
        private List<PokemonDetailDto> _pokemons = new();
        private List<ItemDto> _items = new();

        public JsonDatabaseContext()
        {
            // Positioned inside PokemonApp/Database/pokemon_db.json
            var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }
            _filePath = Path.Combine(baseDir, "pokemon_db.json");
        }

        public List<PokemonDetailDto> Pokemons
        {
            get { lock (_lock) { return _pokemons.ToList(); } }
        }

        public List<ItemDto> Items
        {
            get { lock (_lock) { return _items.ToList(); } }
        }

        public async Task InitializeAsync(HttpClient client)
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    string json;
                    lock (_lock)
                    {
                        json = File.ReadAllText(_filePath);
                    }
                    var store = JsonSerializer.Deserialize<DatabaseStore>(json);
                    if (store != null)
                    {
                        lock (_lock)
                        {
                            _pokemons = store.Pokemons;
                            _items = store.Items;
                        }
                        Console.WriteLine($"Database loaded successfully: {_pokemons.Count} pokemons and {_items.Count} items.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading database file: {ex.Message}. Re-seeding...");
                }
            }

            // Seed database
            await SeedDatabaseAsync(client);
        }

        private async Task SeedDatabaseAsync(HttpClient client)
        {
            Console.WriteLine("Seeding local JSON database from PokeAPI...");
            client.DefaultRequestHeaders.Add("User-Agent", "PokeAppNET10DbSeeder");

            var seededPokemons = new List<PokemonDetailDto>();
            var seededItems = new List<ItemDto>();

            try
            {
                // 1. Fetch 151 original Pokemon (Gen 1) to keep seeding quick but comprehensive
                var listResponse = await client.GetFromJsonAsync<PokeApiNamedResourceList>("https://pokeapi.co/api/v2/pokemon?limit=151");
                if (listResponse != null && listResponse.Results.Count > 0)
                {
                    var tasks = listResponse.Results.Select(async resource =>
                    {
                        try
                        {
                            var detail = await client.GetFromJsonAsync<PokeApiPokemonDetail>(resource.Url);
                            if (detail != null)
                            {
                                var statsMap = detail.Stats.ToDictionary(s => s.Stat.Name, s => s.BaseStat);
                                var normalizedStats = new Dictionary<string, int>
                                {
                                    { "hp", statsMap.GetValueOrDefault("hp", 50) },
                                    { "attack", statsMap.GetValueOrDefault("attack", 50) },
                                    { "defense", statsMap.GetValueOrDefault("defense", 50) },
                                    { "specialAttack", statsMap.GetValueOrDefault("special-attack", 50) },
                                    { "specialDefense", statsMap.GetValueOrDefault("special-defense", 50) },
                                    { "speed", statsMap.GetValueOrDefault("speed", 50) }
                                };

                                // Get species details for flavor text
                                string description = "No description available.";
                                try
                                {
                                    var species = await client.GetFromJsonAsync<PokeApiPokemonSpecies>($"https://pokeapi.co/api/v2/pokemon-species/{detail.Id}/");
                                    if (species != null && species.FlavorTextEntries.Any())
                                    {
                                        var englishEntry = species.FlavorTextEntries.FirstOrDefault(e => e.Language.Name == "en");
                                        if (englishEntry != null)
                                        {
                                            description = englishEntry.FlavorText.Replace("\n", " ").Replace("\f", " ").Replace("\r", " ");
                                        }
                                    }
                                }
                                catch {}

                                var dto = new PokemonDetailDto
                                {
                                    Id = detail.Id,
                                    Name = detail.Name,
                                    ImageUrl = detail.Sprites.Other.OfficialArtwork.FrontDefault ?? $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{detail.Id}.png",
                                    Types = detail.Types.OrderBy(t => t.Slot).Select(t => t.Type.Name).ToList(),
                                    Description = description,
                                    Height = detail.Height / 10.0,
                                    Weight = detail.Weight / 10.0,
                                    Abilities = detail.Abilities.Select(a => a.Ability.Name).ToList(),
                                    Stats = normalizedStats
                                };

                                lock (seededPokemons)
                                {
                                    seededPokemons.Add(dto);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error seeding individual pokemon {resource.Name}: {ex.Message}");
                        }
                    });

                    await Task.WhenAll(tasks);
                }

                // 2. Fetch 50 items
                var itemsResponse = await client.GetFromJsonAsync<PokeApiNamedResourceList>("https://pokeapi.co/api/v2/item?limit=50");
                if (itemsResponse != null && itemsResponse.Results.Count > 0)
                {
                    var tasks = itemsResponse.Results.Select(async resource =>
                    {
                        try
                        {
                            var detail = await client.GetFromJsonAsync<PokeApiItemDetail>(resource.Url);
                            if (detail != null)
                            {
                                string effect = "No description available.";
                                if (detail.EffectEntries.Any())
                                {
                                    var englishEntry = detail.EffectEntries.FirstOrDefault(e => e.Language.Name == "en");
                                    if (englishEntry != null)
                                    {
                                        effect = englishEntry.Effect.Replace("\n", " ").Replace("\r", " ");
                                    }
                                }

                                var dto = new ItemDto
                                {
                                    Id = detail.Id,
                                    Name = detail.Name,
                                    Category = detail.Category.Name,
                                    Effect = effect,
                                    Cost = detail.Cost,
                                    ImageUrl = detail.Sprites?.Default ?? $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/items/{detail.Name}.png"
                                };

                                lock (seededItems)
                                {
                                    seededItems.Add(dto);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error seeding individual item {resource.Name}: {ex.Message}");
                        }
                    });

                    await Task.WhenAll(tasks);
                }

                // Sort lists
                var sortedPokemons = seededPokemons.OrderBy(p => p.Id).ToList();
                var sortedItems = seededItems.OrderBy(i => i.Id).ToList();

                lock (_lock)
                {
                    _pokemons = sortedPokemons;
                    _items = sortedItems;
                }

                SaveChanges();
                Console.WriteLine($"Seeding complete! Saved {_pokemons.Count} pokemons and {_items.Count} items to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database seeding failed: {ex.Message}");
            }
        }

        public void SaveChanges()
        {
            lock (_lock)
            {
                try
                {
                    var store = new DatabaseStore { Pokemons = _pokemons, Items = _items };
                    var json = JsonSerializer.Serialize(store, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(_filePath, json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing to database file: {ex.Message}");
                }
            }
        }

        // CRUD for Pokémon
        public void AddPokemon(PokemonDetailDto pokemon)
        {
            lock (_lock)
            {
                // Generate next available ID if needed
                if (pokemon.Id <= 0)
                {
                    pokemon.Id = _pokemons.Any() ? _pokemons.Max(p => p.Id) + 1 : 1;
                }
                _pokemons.Add(pokemon);
                SaveChanges();
            }
        }

        public bool UpdatePokemon(int id, PokemonDetailDto pokemon)
        {
            lock (_lock)
            {
                var existing = _pokemons.FirstOrDefault(p => p.Id == id);
                if (existing == null) return false;

                existing.Name = pokemon.Name;
                existing.ImageUrl = pokemon.ImageUrl;
                existing.Types = pokemon.Types;
                existing.Description = pokemon.Description;
                existing.Height = pokemon.Height;
                existing.Weight = pokemon.Weight;
                existing.Abilities = pokemon.Abilities;
                existing.Stats = pokemon.Stats;

                SaveChanges();
                return true;
            }
        }

        public bool DeletePokemon(int id)
        {
            lock (_lock)
            {
                var existing = _pokemons.FirstOrDefault(p => p.Id == id);
                if (existing == null) return false;

                _pokemons.Remove(existing);
                SaveChanges();
                return true;
            }
        }
    }
}
