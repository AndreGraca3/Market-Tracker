using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Operations.Category;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Category;

using Category = market_tracker_webapi.Application.Domain.Category;

public class CategoryService(
    ICategoryRepository categoryRepository,
    ITransactionManager transactionManager
) : ICategoryService
{
    public async Task<CollectionOutputModel> GetCategoriesAsync()
    {
        var categories = await categoryRepository.GetCategoriesAsync();
        return new CollectionOutputModel(categories);
    }

    public async Task<Either<CategoryFetchingError, Category>> GetCategoryAsync(int id)
    {
        var category = await categoryRepository.GetCategoryByIdAsync(id);
        return category is null
            ? EitherExtensions.Failure<CategoryFetchingError, Category>(
                new CategoryFetchingError.CategoryByIdNotFound(id)
            )
            : EitherExtensions.Success<CategoryFetchingError, Category>(category);
    }

    public async Task<Either<ICategoryError, IdOutputModel>> AddCategoryAsync(string name)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await categoryRepository.GetCategoryByNameAsync(name) is not null)
            {
                return EitherExtensions.Failure<ICategoryError, IdOutputModel>(
                    new CategoryCreationError.CategoryNameAlreadyExists(name)
                );
            }

            var categoryId = await categoryRepository.AddCategoryAsync(name);
            return EitherExtensions.Success<ICategoryError, IdOutputModel>(
                new IdOutputModel(categoryId)
            );
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

    public async Task<Either<CategoryFetchingError, IdOutputModel>> RemoveCategoryAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var category = await categoryRepository.RemoveCategoryAsync(id);
            return category is null
                ? EitherExtensions.Failure<CategoryFetchingError, IdOutputModel>(
                    new CategoryFetchingError.CategoryByIdNotFound(id)
                )
                : EitherExtensions.Success<CategoryFetchingError, IdOutputModel>(
                    new IdOutputModel(category.Id)
                );
        });
    }
}
