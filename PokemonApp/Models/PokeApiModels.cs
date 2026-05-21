using System.Text.Json.Serialization;

namespace PokemonApp.Models
{
    public class PokeApiNamedResourceList
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("next")]
        public string? Next { get; set; }

        [JsonPropertyName("results")]
        public List<PokeApiNamedResource> Results { get; set; } = new();
    }

    public class PokeApiNamedResource
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class PokeApiPokemonDetail
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        [JsonPropertyName("types")]
        public List<PokeApiPokemonTypeSlot> Types { get; set; } = new();

        [JsonPropertyName("stats")]
        public List<PokeApiPokemonStatSlot> Stats { get; set; } = new();

        [JsonPropertyName("abilities")]
        public List<PokeApiPokemonAbilitySlot> Abilities { get; set; } = new();

        [JsonPropertyName("sprites")]
        public PokeApiPokemonSprites Sprites { get; set; } = new();
    }

    public class PokeApiPokemonTypeSlot
    {
        [JsonPropertyName("slot")]
        public int Slot { get; set; }

        [JsonPropertyName("type")]
        public PokeApiNamedResource Type { get; set; } = new();
    }

    public class PokeApiPokemonStatSlot
    {
        [JsonPropertyName("base_stat")]
        public int BaseStat { get; set; }

        [JsonPropertyName("stat")]
        public PokeApiNamedResource Stat { get; set; } = new();
    }

    public class PokeApiPokemonAbilitySlot
    {
        [JsonPropertyName("ability")]
        public PokeApiNamedResource Ability { get; set; } = new();

        [JsonPropertyName("is_hidden")]
        public bool IsHidden { get; set; }
    }

    public class PokeApiPokemonSprites
    {
        [JsonPropertyName("other")]
        public PokeApiPokemonOtherSprites Other { get; set; } = new();
    }

    public class PokeApiPokemonOtherSprites
    {
        [JsonPropertyName("official-artwork")]
        public PokeApiPokemonOfficialArtwork OfficialArtwork { get; set; } = new();
    }

    public class PokeApiPokemonOfficialArtwork
    {
        [JsonPropertyName("front_default")]
        public string FrontDefault { get; set; } = string.Empty;
    }

    public class PokeApiPokemonSpecies
    {
        [JsonPropertyName("flavor_text_entries")]
        public List<PokeApiFlavorTextEntry> FlavorTextEntries { get; set; } = new();
    }

    public class PokeApiFlavorTextEntry
    {
        [JsonPropertyName("flavor_text")]
        public string FlavorText { get; set; } = string.Empty;

        [JsonPropertyName("language")]
        public PokeApiNamedResource Language { get; set; } = new();
    }

    public class PokeApiItemDetail
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("cost")]
        public int Cost { get; set; }

        [JsonPropertyName("effect_entries")]
        public List<PokeApiItemEffectEntry> EffectEntries { get; set; } = new();

        [JsonPropertyName("category")]
        public PokeApiNamedResource Category { get; set; } = new();

        [JsonPropertyName("sprites")]
        public PokeApiItemSprites Sprites { get; set; } = new();
    }

    public class PokeApiItemSprites
    {
        [JsonPropertyName("default")]
        public string Default { get; set; } = string.Empty;
    }

    public class PokeApiItemEffectEntry
    {
        [JsonPropertyName("effect")]
        public string Effect { get; set; } = string.Empty;

        [JsonPropertyName("language")]
        public PokeApiNamedResource Language { get; set; } = new();
    }
}
