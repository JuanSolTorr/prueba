namespace PokemonApp.Models
{
    public class PokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<string> Types { get; set; } = new();
    }

    public class PokemonDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<string> Types { get; set; } = new();
        public string Description { get; set; } = string.Empty;
        public double Height { get; set; } // in meters
        public double Weight { get; set; } // in kg
        public List<string> Abilities { get; set; } = new();
        public Dictionary<string, int> Stats { get; set; } = new();
    }
}
