using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto;

namespace market_tracker_webapi.Application.Repository.Interfaces;

public interface ICategoryRepository
{
    public Task<List<Category>> GetCategoriesAsync();

    public Task<Category?> GetCategoryByIdAsync(int id);

    public Task<CategoryItem?> GetCategoryByNameAsync(string name);

    public Task<CategoryItem> AddCategoryAsync(string name, int? parentId);

    public Task<CategoryItem?> RemoveCategoryAsync(int id);
}
