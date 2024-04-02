namespace market_tracker_webapi.Application.Service.Errors.Category;

public class CategoryFetchingError : ICategoryError
{
    public class CategoryByIdNotFound(int id) : CategoryFetchingError
    {
        public int Id { get; } = id;
    }
}
