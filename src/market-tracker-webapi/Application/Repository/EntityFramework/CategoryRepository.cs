using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Interfaces;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.EntityFramework;

public class CategoryRepository(MarketTrackerDataContext dataContext) : ICategoryRepository
{
    public Task<List<Category>> GetCategoriesAsync()
    {
        return Task.FromResult(
            dataContext
                .Category.Where(c => c.ParentId == null)
                .Select(pc => new Category(
                    pc.Id,
                    pc.Name,
                    pc.ParentId,
                    dataContext
                        .Category.Where(c => c.ParentId == pc.Id)
                        .Select(c => new Category(c.Id, c.Name, c.ParentId, new List<Category>()))
                        .ToList()
                ))
                .ToList()
        );
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        var categoryEntity = await dataContext.Category.FindAsync(id);
        if (categoryEntity is null)
        {
            return null;
        }
        return new Category(
            categoryEntity.Id,
            categoryEntity.Name,
            categoryEntity.ParentId,
            dataContext
                .Category.Where(c => c.ParentId == categoryEntity.Id)
                .Select(c => new Category(c.Id, c.Name, c.ParentId, new List<Category>()))
                .ToList()
        );
    }

    public async Task<CategoryItem?> GetCategoryByNameAsync(string name)
    {
        var categoryEntity = await dataContext.Category.FirstOrDefaultAsync(c => c.Name == name);
        return categoryEntity is null
            ? null
            : new CategoryItem(categoryEntity.Id, categoryEntity.Name, categoryEntity.ParentId);
    }

    public async Task<CategoryItem> AddCategoryAsync(string name, int? parentId)
    {
        var category = new CategoryEntity { Name = name, ParentId = parentId };
        await dataContext.Category.AddAsync(category);
        await dataContext.SaveChangesAsync();
        return new CategoryItem(category.Id, category.Name, parentId);
    }

    public async Task<CategoryItem?> RemoveCategoryAsync(int id)
    {
        var category = await dataContext.Category.FindAsync(id);
        if (category is null)
        {
            return null;
        }
        dataContext.Category.Remove(category);
        await dataContext.SaveChangesAsync();
        return new CategoryItem(category.Id, category.Name, null);
    }
}
