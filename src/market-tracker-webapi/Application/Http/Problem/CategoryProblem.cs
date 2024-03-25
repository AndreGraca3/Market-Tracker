using market_tracker_webapi.Application.Service.Errors.Category;

namespace market_tracker_webapi.Application.Http.Problem;

public class CategoryProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class CategoryByIdNotFound(CategoryFetchingError.CategoryByIdNotFound data)
        : CategoryProblem(
            404,
            "category-not-found",
            "Category not found",
            $"Category with id {data.Id} not found",
            data
        );

    public class CategoryNameAlreadyExists()
        : CategoryProblem(
            409,
            "category-name-already-exists",
            "Category name already exists",
            "A category with that name already exists"
        );

    public class InvalidName(CategoryCreationError.InvalidName data)
        : CategoryProblem(
            400,
            "invalid-name",
            "Invalid name",
            $"Name must be between {data.MinCategoryNameLength} and {data.MaxCategoryNameLength} characters long",
            data
        );

    public class InvalidParentCategory(CategoryCreationError.InvalidParentCategory data)
        : CategoryProblem(
            400,
            "invalid-parent-category",
            "Invalid parent category",
            $"Parent category must be a root category",
            data
        );
}
