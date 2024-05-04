using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Market.Inventory.Category;

using Category = Domain.Category;

public class CategoryRepository(MarketTrackerDataContext dataContext) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        var categories = await dataContext.Category.ToListAsync();
        return categories.Select(c => new Category(c.Id, c.Name)).ToList();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        var categoryEntity = await dataContext.Category.FindAsync(id);
        if (categoryEntity is null)
        {
            return null;
        }
        return new Category(categoryEntity.Id, categoryEntity.Name);
    }

    public async Task<Category?> GetCategoryByNameAsync(string name)
    {
        var categoryEntity = await dataContext.Category.FirstOrDefaultAsync(c => c.Name == name);
        return categoryEntity is null ? null : new Category(categoryEntity.Id, categoryEntity.Name);
    }

    public async Task<int> AddCategoryAsync(string name)
    {
        var category = new CategoryEntity { Name = name };
        await dataContext.Category.AddAsync(category);
        await dataContext.SaveChangesAsync();
        return category.Id;
    }

    public async Task<Category?> UpdateCategoryAsync(int id, string name)
    {
        var category = await dataContext.Category.FindAsync(id);
        if (category is null)
        {
            return null;
        }
        category.Name = name;
        await dataContext.SaveChangesAsync();
        return new Category(category.Id, category.Name);
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
