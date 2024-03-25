using market_tracker_webapi.Application.Domain;
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
    public async Task<List<Category>> GetCategoriesAsync()
    {
        var categories = await categoryRepository.GetCategoriesAsync();
        return categories
            .Select(category => new Category(
                category.Id,
                category.Name,
                category.ParentId,
                category.Children
            ))
            .ToList();
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

    public async Task<Either<ICategoryError, IdOutputModel>> AddCategoryAsync(
        string name,
        int? parentId
    )
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

        if (parentId != null)
        {
            var parentCategory = await categoryRepository.GetCategoryByIdAsync(parentId.Value);
            if (parentCategory is null)
            {
                return EitherExtensions.Failure<ICategoryError, IdOutputModel>(
                    new CategoryFetchingError.CategoryByIdNotFound(parentId.Value)
                );
            }

            if (parentCategory.ParentId != null)
            {
                return EitherExtensions.Failure<ICategoryError, IdOutputModel>(
                    new CategoryCreationError.InvalidParentCategory(parentId.Value)
                );
            }
        }

        var category = await categoryRepository.AddCategoryAsync(name, parentId);
        return EitherExtensions.Success<ICategoryError, IdOutputModel>(
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
