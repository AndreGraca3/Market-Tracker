namespace market_tracker_webapi.Application.Service.Errors.Category;

public class CategoryCreationError : ICategoryError
{
    public class CategoryNameAlreadyExists(string name) : CategoryCreationError
    {
        public string Name { get; } = name;
    }

    public class InvalidParentCategory(int parentId) : CategoryCreationError
    {
        public int ParentId { get; } = parentId;
    }
}
