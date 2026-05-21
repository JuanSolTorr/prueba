using Microsoft.AspNetCore.Mvc;
using PokemonApp.Models;
using PokemonApp.Services;

namespace PokemonApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search, [FromQuery] string? type, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var pokemons = await _pokemonService.GetAllPokemonsAsync();

            // Filter by search query (name or id)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchClean = search.Trim().ToLower();
                pokemons = pokemons.Where(p => 
                    p.Name.Contains(searchClean, StringComparison.OrdinalIgnoreCase) || 
                    p.Id.ToString() == searchClean
                ).ToList();
            }

            // Filter by type
            if (!string.IsNullOrWhiteSpace(type))
            {
                var typeClean = type.Trim().ToLower();
                pokemons = pokemons.Where(p => 
                    p.Types.Any(t => t.Equals(typeClean, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            var totalCount = pokemons.Count;
            var paginatedItems = pokemons
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = paginatedItems
            });
        }

        [HttpGet("{idOrName}")]
        public async Task<IActionResult> GetByIdOrName(string idOrName)
        {
            if (string.IsNullOrWhiteSpace(idOrName))
            {
                return BadRequest("Pokemon ID or Name must be provided.");
            }

            var pokemon = await _pokemonService.GetPokemonByIdOrNameAsync(idOrName);
            if (pokemon == null)
            {
                return NotFound($"Pokemon '{idOrName}' not found.");
            }

            return Ok(pokemon);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PokemonDetailDto pokemon)
        {
            if (pokemon == null || string.IsNullOrWhiteSpace(pokemon.Name))
            {
                return BadRequest("Invalid Pokemon data. Name is required.");
            }

            // Capitalize first letter of Name
            pokemon.Name = char.ToUpper(pokemon.Name[0]) + pokemon.Name[1..].ToLower();

            // Make sure the image is set, otherwise use a placeholder Pokéball sprite
            if (string.IsNullOrWhiteSpace(pokemon.ImageUrl))
            {
                pokemon.ImageUrl = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/items/poke-ball.png";
            }

            // Ensure stats map contains default values if missing
            var stats = pokemon.Stats ?? new Dictionary<string, int>();
            foreach (var key in new[] { "hp", "attack", "defense", "specialAttack", "specialDefense", "speed" })
            {
                if (!stats.ContainsKey(key))
                {
                    stats[key] = 50;
                }
            }
            pokemon.Stats = stats;

            await _pokemonService.CreatePokemonAsync(pokemon);
            return CreatedAtAction(nameof(GetByIdOrName), new { idOrName = pokemon.Id.ToString() }, pokemon);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PokemonDetailDto pokemon)
        {
            if (pokemon == null || string.IsNullOrWhiteSpace(pokemon.Name))
            {
                return BadRequest("Invalid Pokemon data. Name is required.");
            }

            pokemon.Name = char.ToUpper(pokemon.Name[0]) + pokemon.Name[1..].ToLower();

            var success = await _pokemonService.UpdatePokemonAsync(id, pokemon);
            if (!success)
            {
                return NotFound($"Pokemon with ID {id} not found.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _pokemonService.DeletePokemonAsync(id);
            if (!success)
            {
                return NotFound($"Pokemon with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
