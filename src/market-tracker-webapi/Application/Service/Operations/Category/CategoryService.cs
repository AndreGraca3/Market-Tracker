using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Operations.Category;
using market_tracker_webapi.Application.Service.Core;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Category;

using Category = market_tracker_webapi.Application.Domain.Category;

public class CategoryService(
    CategoryManager categoryManager,
    ICategoryRepository categoryRepository,
    TransactionManager transactionManager
) : ICategoryService
{
    public async Task<List<Category>> GetCategoriesAsync()
    {
        var categories = await categoryRepository.GetCategoriesAsync();
        return categories.Select(category => new Category(category.Id, category.Name)).ToList();
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
            if (!categoryManager.IsValidCategoryName(name))
            {
                return EitherExtensions.Failure<ICategoryError, IdOutputModel>(
                    new CategoryCreationError.InvalidName(
                        name,
                        categoryManager.MinCategoryNameLength,
                        categoryManager.MaxCategoryNameLength
                    )
                );
            }

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
