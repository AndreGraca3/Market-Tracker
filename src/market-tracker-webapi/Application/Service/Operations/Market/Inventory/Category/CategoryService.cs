using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Repository.Market.Inventory.Category;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Transaction;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Category;

using Category = Domain.Schemas.Market.Inventory.Category;

public class CategoryService(
    ICategoryRepository categoryRepository,
    ITransactionManager transactionManager
) : ICategoryService
{
    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await transactionManager.ExecuteAsync(async () => await categoryRepository.GetCategoriesAsync());
    }

    public async Task<Category> GetCategoryAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await categoryRepository.GetCategoryByIdAsync(id) ??
            throw new MarketTrackerServiceException(
                new CategoryFetchingError.CategoryByIdNotFound(id)
            ));
    }

    public async Task<CategoryId> AddCategoryAsync(string name)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await categoryRepository.GetCategoryByNameAsync(name) is not null)
            {
                throw new MarketTrackerServiceException(
                    new CategoryCreationError.CategoryNameAlreadyExists(name)
                );
            }

            return await categoryRepository.AddCategoryAsync(name);
        });
    }

    public async Task<Category> UpdateCategoryAsync(int id, string name)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await categoryRepository.GetCategoryByNameAsync(name) is not null)
            {
                throw new MarketTrackerServiceException(
                    new CategoryCreationError.CategoryNameAlreadyExists(name)
                );
            }

            return await categoryRepository.UpdateCategoryAsync(id, name) ?? throw new MarketTrackerServiceException(
                new CategoryFetchingError.CategoryByIdNotFound(id)
            );
        });
    }

    public async Task<CategoryId> RemoveCategoryAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var category = await categoryRepository.RemoveCategoryAsync(id);
            return category is null
                ? throw new MarketTrackerServiceException(
                    new CategoryFetchingError.CategoryByIdNotFound(id)
                )
                : category.Id;
        });
    }
}