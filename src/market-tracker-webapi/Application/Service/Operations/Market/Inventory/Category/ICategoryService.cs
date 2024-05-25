using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Category;

using Category = Domain.Schemas.Market.Inventory.Category;

public interface ICategoryService
{
    public Task<IEnumerable<Category>> GetCategoriesAsync();

    public Task<Category> GetCategoryAsync(int id);

    public Task<CategoryId> AddCategoryAsync(string name);

    public Task<Category> UpdateCategoryAsync(int id, string name);

    public Task<CategoryId> RemoveCategoryAsync(int id);
}
