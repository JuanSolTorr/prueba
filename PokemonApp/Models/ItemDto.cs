namespace PokemonApp.Models
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Effect { get; set; } = string.Empty;
        public int Cost { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
