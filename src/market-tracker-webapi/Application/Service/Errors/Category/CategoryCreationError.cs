namespace market_tracker_webapi.Application.Service.Errors.Category;

public class CategoryCreationError : ICategoryError
{
    public class CategoryNameAlreadyExists(string name) : CategoryCreationError
    {
        public string Name { get; } = name;
    }

    public class InvalidName(string name, int minCategoryNameLength, int maxCategoryNameLength)
        : CategoryCreationError
    {
        public string Name { get; } = name;
        public int MinCategoryNameLength { get; } = minCategoryNameLength;
        public int MaxCategoryNameLength { get; } = maxCategoryNameLength;
    }

    public class InvalidParentCategory(int parentId) : CategoryCreationError
    {
        public int ParentId { get; } = parentId;
    }
}
