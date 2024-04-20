namespace market_tracker_webapi.Application.Service.Errors.List;

public class ListFetchingError : IListError
{
    public class ListByIdNotFound(int id) : ListFetchingError
    {
        public int Id { get; } = id;
    }

    public class UserDoesNotOwnList(Guid clientId, int listId) : ListFetchingError
    {
        public Guid ClientId { get; } = clientId;
        public int ListId { get; } = listId;
    }
    
}