using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Interfaces;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.EntityFramework;

public class CategoryRepository(MarketTrackerDataContext dataContext) : ICategoryRepository
{
    public async Task<List<CategoryEntity>> GetCategoriesAsync()
    {
        return await dataContext.Category.ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        var categoryEntity = await dataContext.Category.FindAsync(id);
        return categoryEntity is not null
            ? new Category(categoryEntity.Id, categoryEntity.Name)
            : null;
    }

    public async Task<Category?> GetCategoryByNameAsync(string name)
    {
        var categoryEntity = await dataContext.Category.FirstOrDefaultAsync(category =>
            category.Name == name
        );
        return categoryEntity is not null
            ? new Category(categoryEntity.Id, categoryEntity.Name)
            : null;
    }

    public async Task<CategoryEntity> AddCategoryAsync(string name)
    {
        var category = new CategoryEntity { Name = name };
        return (await dataContext.Category.AddAsync(category)).Entity;
    }

    public async Task<Category?> RemoveCategoryAsync(int id)
    {
        var category = await dataContext.Category.FindAsync(id);
        if (category is null)
        {
            return null;
        }
        dataContext.Category.Remove(category);
        return new Category(category.Id, category.Name);
    }
}
