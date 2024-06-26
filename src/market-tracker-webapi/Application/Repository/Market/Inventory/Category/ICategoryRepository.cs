using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;

namespace market_tracker_webapi.Application.Repository.Market.Inventory.Category;

using Category = Domain.Schemas.Market.Inventory.Category;

public interface ICategoryRepository
{
    public Task<IEnumerable<Category>> GetCategoriesAsync();

    public Task<Category?> GetCategoryByIdAsync(int id);

    public Task<Category?> GetCategoryByNameAsync(string name);

    public Task<CategoryId> AddCategoryAsync(string name);

    public Task<Category?> UpdateCategoryAsync(int id, string name);

    public Task<Category?> RemoveCategoryAsync(int id);
}
