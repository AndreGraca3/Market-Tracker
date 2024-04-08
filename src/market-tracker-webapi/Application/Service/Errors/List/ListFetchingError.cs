namespace market_tracker_webapi.Application.Service.Errors.List;

public class ListFetchingError : IListError
{
    public class ListByIdNotFound(int id) : ListFetchingError
    {
        public int Id { get; } = id;
    }
    
}