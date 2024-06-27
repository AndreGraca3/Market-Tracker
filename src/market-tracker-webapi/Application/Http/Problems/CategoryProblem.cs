using market_tracker_webapi.Application.Service.Errors.Category;

namespace market_tracker_webapi.Application.Http.Problems;

public class CategoryProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problems.Problem(status, subType, title, detail, data)
{
    public class CategoryByIdNotFound(CategoryFetchingError.CategoryByIdNotFound data)
        : CategoryProblem(
            404,
            "category-not-found",
            "Category not found",
            "Category does not exist",
            data
        );

    public class CategoryNameAlreadyExists()
        : CategoryProblem(
            409,
            "category-name-already-exists",
            "Category name already exists",
            "A category with that name already exists"
        );

    public class InvalidParentCategory(CategoryCreationError.InvalidParentCategory data)
        : CategoryProblem(
            400,
            "invalid-parent-category",
            "Invalid parent category", 
            "Parent category must be a root category",
            data
        );

    public static CategoryProblem FromServiceError(ICategoryError error)
    {
        return error switch
        {
            CategoryFetchingError.CategoryByIdNotFound categoryByIdNotFound => new CategoryByIdNotFound(categoryByIdNotFound),
            CategoryCreationError.CategoryNameAlreadyExists => new CategoryNameAlreadyExists(),
            CategoryCreationError.InvalidParentCategory invalidParentCategory => new InvalidParentCategory(invalidParentCategory),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
    }
}
