using market_tracker_webapi.Application.Domain.Models.Market.Inventory;
using market_tracker_webapi.Application.Repository.Market.Inventory.Category;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Category;

using Category = Domain.Models.Market.Inventory.Category;

public class CategoryService(
    ICategoryRepository categoryRepository,
    ITransactionManager transactionManager
) : ICategoryService
{
    public async Task<Either<IServiceError, IEnumerable<Category>>> GetCategoriesAsync()
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var categories = await categoryRepository.GetCategoriesAsync();
            return EitherExtensions.Success<IServiceError, IEnumerable<Category>>(categories);
        });
    }

    public async Task<Either<CategoryFetchingError, Category>> GetCategoryAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var category = await categoryRepository.GetCategoryByIdAsync(id);
            return category is null
                ? EitherExtensions.Failure<CategoryFetchingError, Category>(
                    new CategoryFetchingError.CategoryByIdNotFound(id)
                )
                : EitherExtensions.Success<CategoryFetchingError, Category>(category);
        });
    }

    public async Task<Either<ICategoryError, CategoryId>> AddCategoryAsync(string name)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await categoryRepository.GetCategoryByNameAsync(name) is not null)
            {
                return EitherExtensions.Failure<ICategoryError, CategoryId>(
                    new CategoryCreationError.CategoryNameAlreadyExists(name)
                );
            }

            var categoryId = await categoryRepository.AddCategoryAsync(name);
            return EitherExtensions.Success<ICategoryError, CategoryId>(categoryId);
        });
    }

    public async Task<Either<ICategoryError, Category>> UpdateCategoryAsync(int id, string name)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await categoryRepository.GetCategoryByNameAsync(name) is not null)
            {
                return EitherExtensions.Failure<ICategoryError, Category>(
                    new CategoryCreationError.CategoryNameAlreadyExists(name)
                );
            }

            var newCategory = await categoryRepository.UpdateCategoryAsync(id, name);

            if (newCategory is null)
            {
                return EitherExtensions.Failure<ICategoryError, Category>(
                    new CategoryFetchingError.CategoryByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<ICategoryError, Category>(newCategory);
        });
    }

    public async Task<Either<CategoryFetchingError, CategoryId>> RemoveCategoryAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var category = await categoryRepository.RemoveCategoryAsync(id);
            return category is null
                ? EitherExtensions.Failure<CategoryFetchingError, CategoryId>(
                    new CategoryFetchingError.CategoryByIdNotFound(id)
                )
                : EitherExtensions.Success<CategoryFetchingError, CategoryId>(
                    category.Id
                );
        });
    }
}