using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Interfaces;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.EntityFramework;

public class CategoryRepository(MarketTrackerDataContext dataContext) : ICategoryRepository
{
    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await dataContext.Category
            .Select(category => new Category(category.Id, category.Name, category.ParentId))
            .ToListAsync();
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

    public async Task<Category> AddCategoryAsync(string name, int? parentId)
    {
        var category = new CategoryEntity { Name = name, ParentId = parentId };
        await dataContext.Category.AddAsync(category);
        await dataContext.SaveChangesAsync();
        return new Category(category.Id, category.Name, parentId);
    }

    public async Task<Category?> RemoveCategoryAsync(int id)
    {
        var category = await dataContext.Category.FindAsync(id);
        if (category is null)
        {
            return null;
        }
        dataContext.Category.Remove(category);
        await dataContext.SaveChangesAsync();
        return new Category(category.Id, category.Name);
    }
}
