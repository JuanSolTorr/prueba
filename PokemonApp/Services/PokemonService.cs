using PokemonApp.Database;
using PokemonApp.Models;

namespace PokemonApp.Services
{
    public interface IPokemonService
    {
        Task<List<PokemonDetailDto>> GetAllPokemonsAsync();
        Task<PokemonDetailDto?> GetPokemonByIdOrNameAsync(string idOrName);
        Task CreatePokemonAsync(PokemonDetailDto pokemon);
        Task<bool> UpdatePokemonAsync(int id, PokemonDetailDto pokemon);
        Task<bool> DeletePokemonAsync(int id);
    }

    public class PokemonService : IPokemonService
    {
        private readonly JsonDatabaseContext _dbContext;

        public PokemonService(JsonDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<PokemonDetailDto>> GetAllPokemonsAsync()
        {
            // Returns the list of Pokemons from the local database
            return Task.FromResult(_dbContext.Pokemons);
        }

        public Task<PokemonDetailDto?> GetPokemonByIdOrNameAsync(string idOrName)
        {
            var pokemons = _dbContext.Pokemons;
            PokemonDetailDto? pokemon;

            if (int.TryParse(idOrName, out int id))
            {
                pokemon = pokemons.FirstOrDefault(p => p.Id == id);
            }
            else
            {
                pokemon = pokemons.FirstOrDefault(p => p.Name.Equals(idOrName, StringComparison.OrdinalIgnoreCase));
            }

            return Task.FromResult(pokemon);
        }

        public Task CreatePokemonAsync(PokemonDetailDto pokemon)
        {
            _dbContext.AddPokemon(pokemon);
            return Task.CompletedTask;
        }

        public Task<bool> UpdatePokemonAsync(int id, PokemonDetailDto pokemon)
        {
            var success = _dbContext.UpdatePokemon(id, pokemon);
            return Task.FromResult(success);
        }

        public Task<bool> DeletePokemonAsync(int id)
        {
            var success = _dbContext.DeletePokemon(id);
            return Task.FromResult(success);
        }
    }
}
