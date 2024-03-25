using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Interfaces;

public interface ICategoryRepository
{
    public Task<List<Category>> GetCategoriesAsync();

    public Task<Category?> GetCategoryByIdAsync(int id);

    public Task<Category?> GetCategoryByNameAsync(string name);

    public Task<Category> AddCategoryAsync(string name);

    public Task<Category?> RemoveCategoryAsync(int id);
}
