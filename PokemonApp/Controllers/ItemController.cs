using Microsoft.AspNetCore.Mvc;
using PokemonApp.Services;

namespace PokemonApp.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search, [FromQuery] string? category, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var items = await _itemService.GetAllItemsAsync();

            // Filter by search query (name or id)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchClean = search.Trim().ToLower();
                items = items.Where(i => 
                    i.Name.Contains(searchClean, StringComparison.OrdinalIgnoreCase) || 
                    i.Id.ToString() == searchClean
                ).ToList();
            }

            // Filter by category
            if (!string.IsNullOrWhiteSpace(category))
            {
                var categoryClean = category.Trim().ToLower();
                items = items.Where(i => 
                    i.Category.Contains(categoryClean, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            var totalCount = items.Count;
            var paginatedItems = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Get list of all available categories for filtering options in the UI
            var allCategories = items
                .Select(i => i.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return Ok(new
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = paginatedItems,
                Categories = allCategories
            });
        }
    }
}
