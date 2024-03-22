using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Interfaces;
using market_tracker_webapi.Application.Service.Core;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service;

public class CategoryService(
    CategoryManager categoryManager,
    ICategoryRepository categoryRepository
)
{
    public async Task<List<CategoryOutputModel>> GetCategoriesAsync()
    {
        var categories = await categoryRepository.GetCategoriesAsync();
        return categories
            .Select(category => new CategoryOutputModel(category.Id, category.Name))
            .ToList();
    }

    public async Task<Either<CategoryFetchingError, CategoryOutputModel>> GetCategoryAsync(int id)
    {
        var category = await categoryRepository.GetCategoryByIdAsync(id);
        return category is null
            ? EitherExtensions.Failure<CategoryFetchingError, CategoryOutputModel>(
                new CategoryFetchingError.CategoryByIdNotFound(id)
            )
            : EitherExtensions.Success<CategoryFetchingError, CategoryOutputModel>(
                new CategoryOutputModel(category.Id, category.Name)
            );
    }

    public async Task<Either<CategoryCreationError, IdOutputModel>> AddCategoryAsync(
        string name,
        int? parentId
    )
    {
        /*if (!categoryManager.IsValidCategoryName(name))
        {
            return EitherExtensions.Failure<CategoryCreationError, IdOutputModel>(
                new CategoryCreationError.InvalidName(
                    name,
                    categoryManager.MinCategoryNameLength,
                    categoryManager.MaxCategoryNameLength
                )
            );
        }*/

        if (await categoryRepository.GetCategoryByNameAsync(name) is not null)
        {
            return EitherExtensions.Failure<CategoryCreationError, IdOutputModel>(
                new CategoryCreationError.CategoryNameAlreadyExists(name)
            );
        }

        var category = await categoryRepository.AddCategoryAsync(name, parentId);
        return EitherExtensions.Success<CategoryCreationError, IdOutputModel>(
            new IdOutputModel(category.Id)
        );
    }

    public async Task<Either<CategoryFetchingError, IdOutputModel>> RemoveCategoryAsync(int id)
    {
        var category = await categoryRepository.RemoveCategoryAsync(id);
        return category is null
            ? EitherExtensions.Failure<CategoryFetchingError, IdOutputModel>(
                new CategoryFetchingError.CategoryByIdNotFound(id)
            )
            : EitherExtensions.Success<CategoryFetchingError, IdOutputModel>(
                new IdOutputModel(category.Id)
            );
    }
}
