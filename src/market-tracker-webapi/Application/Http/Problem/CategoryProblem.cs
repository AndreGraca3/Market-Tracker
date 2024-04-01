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

    public class InvalidParentCategory(CategoryCreationError.InvalidParentCategory data)
        : CategoryProblem(
            400,
            "invalid-parent-category",
            "Invalid parent category",
            $"Parent category must be a root category",
            data
        );
}
