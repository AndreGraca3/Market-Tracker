namespace market_tracker_webapi.Application.Service.Errors.List;

public class ListFetchingError : IListError
{
    public class ListByIdNotFound(string listId) : ListFetchingError
    {
        public string ListId { get; } = listId;
    }

    public class ClientDoesNotOwnList(Guid clientId, string listId) : ListFetchingError
    {
        public Guid ClientId { get; } = clientId;
        public string ListId { get; } = listId;
    }

    public class ClientDoesNotBelongToList(Guid clientId, string listId) : ListFetchingError
    {
        public Guid ClientId { get; } = clientId;
        public string ListId { get; } = listId;
    }
}