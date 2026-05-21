using PokemonApp.Database;
using PokemonApp.Models;

namespace PokemonApp.Services
{
    public interface IItemService
    {
        Task<List<ItemDto>> GetAllItemsAsync();
    }

    public class ItemService : IItemService
    {
        private readonly JsonDatabaseContext _dbContext;

        public ItemService(JsonDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<ItemDto>> GetAllItemsAsync()
        {
            // Returns the list of items from the local database
            return Task.FromResult(_dbContext.Items);
        }
    }
}
